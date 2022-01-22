﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Bebruber.DataAccess;
using Bebruber.Domain.Entities;
using Bebruber.Domain.Services;
using FluentResults;
using MediatR;

namespace Bebruber.Application.Rides.Commands;

public static class RideFinishedCommand
{
    public record Command(
        Guid RideId) : IRequest<Response>;
    
    public record Response(
        bool Status);

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IRideService _rideService;
        private readonly BebruberDatabaseContext _databaseContext;

        CommandHandler(
            IRideService rideService,
            BebruberDatabaseContext context)
        {
            _rideService = rideService;
            _databaseContext = context;
        }
        
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            Ride? ride = await _databaseContext.Rides.FindAsync(request.RideId);
            if (ride is not null)
            {
                await _rideService.FinishRideAsync(ride, cancellationToken);
            }

            return new Response(ride is not null);
        }   
    }
}