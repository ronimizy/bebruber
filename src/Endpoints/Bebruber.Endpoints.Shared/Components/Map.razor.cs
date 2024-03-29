﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Bebruber.Endpoints.Shared.Models;
using FisSst.BlazorMaps;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Marker = Bebruber.Endpoints.Shared.Models.Marker;
using Polyline = Bebruber.Endpoints.Shared.Models.Polyline;

namespace Bebruber.Endpoints.Shared.Components
{
    public partial class Map
    {
        private FisSst.BlazorMaps.Map _mapObject;

        private MapOptions _options = new MapOptions()
        {
            DivId = "Map",
            Center = new LatLng(59.956175868546254, 30.309461702204565),
            Zoom = 13,
            UrlTileLayer = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png",
            SubOptions = new MapSubOptions()
            {
                Attribution = "&copy; <a lhref='http://www.openstreetmap.org/copyright'>OpenStreetMap</a>",
                TileSize = 512,
                ZoomOffset = -1,
                MaxZoom = 19,
            },
        };

        [Inject]
        public IPolylineFactory PolylineFactory { get; init; }
        [Parameter]
        public string Height { get; set; }
        [Parameter]
        public bool NeedGetMarkerAddress { get; set; }
        [Parameter]
        public bool AddMarkerOnClick { get; set; }
        [Parameter]
        public MakerConfig MarkerConfig { get; set; }
        [Parameter]
        public Action<Marker> OnMarkerAdded { get; set; }
        [Inject]
        private IMarkerFactory MarkerFactory { get; set; }
        [Inject]
        private IIconFactory IconFactory { get; init; }

        public async Task<MapPoint> GetCenterAsync()
        {
            LatLng center = await _mapObject.GetCenter();
            return new MapPoint(center);
        }

        public async Task<Marker> CreateMarker(MapPoint point)
        {
            FisSst.BlazorMaps.Marker marker = await MarkerFactory.CreateAndAddToMap(point.ToLatLng(), _mapObject);
            return new Marker(point, marker);
        }

        public async Task<FisSst.BlazorMaps.Polyline> CreatePolyline(ICollection<MapPoint> points)
        {
            var latLngs = new List<LatLng>(new List<LatLng>(points.Select(p => new LatLng(p.Latitude, p.Longitude))));
            return await PolylineFactory.CreateAndAddToMap(latLngs, _mapObject);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            OnAfterRender(firstRender);
            if (!firstRender)
                return;

            // MouseEvents of Fis.Sst Map are initialized in OnAfterRender method
            // OnAfterRender of child components is called after such method of parent component
            // So we have to wait until MouseEvents are initialized
            while (true)
            {
                try
                {
                    await _mapObject.OnClick(MapOnClick);
                    break;
                }
                catch (NullReferenceException)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(5));
                }
            }
        }

        private async Task<string> GetAddress(LatLng point)
        {
            var httpClient = new HttpClient();
            string latitudeString = point.Lat.ToString(CultureInfo.InvariantCulture);
            string longitudeString = point.Lng.ToString(CultureInfo.InvariantCulture);
            HttpResponseMessage response = await httpClient.GetAsync(
                $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitudeString}&lon={longitudeString}");
            string stringData = await response.Content.ReadAsStringAsync();
            LocationInfo location = JsonConvert.DeserializeObject<LocationInfo>(stringData);
            return location.Address.Road is null ? null : $"{location.Address.Road} {location.Address.HouseNumber}";
        }

        private async Task MapOnClick(MouseEvent mouseEvent)
        {
            if (AddMarkerOnClick)
            {
                string markerAddress = NeedGetMarkerAddress ? await GetAddress(mouseEvent.LatLng) : null;
                FisSst.BlazorMaps.Marker marker = null;
                if (MarkerConfig.IconUrl is not "")
                {
                    var iconOptions = new IconOptions()
                    {
                        IconUrl = MarkerConfig.IconUrl,
                        IconSize = MarkerConfig.IconSize,
                        IconAnchor = MarkerConfig.IconAnchor,
                    };
                    Icon icon = await IconFactory.Create(iconOptions);
                    var markerOptions = new MarkerOptions()
                    {
                        IconRef = icon,
                    };
                    marker = await MarkerFactory.CreateAndAddToMap(mouseEvent.LatLng, _mapObject, markerOptions);
                }
                else
                {
                    marker = await MarkerFactory.CreateAndAddToMap(mouseEvent.LatLng, _mapObject);
                }

                var mapMarker = new Marker(new MapPoint(mouseEvent.LatLng), markerAddress, marker);

                OnMarkerAdded?.Invoke(mapMarker);
            }
        }
    }
}