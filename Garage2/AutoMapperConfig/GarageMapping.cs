using AutoMapper;
using Garage2.Models.Entities;
using Garage2.Models.ViewModels;

namespace Garage2.AutoMapperConfig;

public class GarageMapping : Profile
{
	public GarageMapping()
	{
		CreateMap<ParkedVehicle, ParkedVehiclesViewModel>();
		//CreateMap<ParkedVehicle, VehicleStatistics>();
	
		CreateMap<ParkedVehicle, CheckOutVehicleViewModel>();
	}
    
}