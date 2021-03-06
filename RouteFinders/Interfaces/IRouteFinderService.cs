﻿using Common.Models;
using RouteFinders.Enums;
using RouteFinders.Models;

namespace RouteFinders.Interfaces
{
    public interface IRouteFinderService
    {
        bool IsValidFor(MeanOfTransport meanOfTransport);

        Route GetRoute(Coordinates from, Coordinates to, MeanOfTransport meanOfTransport);
    }
}