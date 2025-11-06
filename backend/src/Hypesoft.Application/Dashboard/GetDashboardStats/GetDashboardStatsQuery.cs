namespace Hypesoft.Application.Dashboard.GetDashboardStats;

using MediatR;
using Hypesoft.Application.DTOs;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;