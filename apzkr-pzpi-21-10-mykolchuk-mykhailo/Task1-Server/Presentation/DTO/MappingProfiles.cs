using AutoMapper;

namespace APZ_backend.DTO
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // For Records
            CreateMap<BrakingMoment, BrakingMomentDto>();
            CreateMap<BrakingMomentDto, BrakingMoment>();

            CreateMap<EngineSpeedMoment, EngineSpeedMomentDto>();
            CreateMap<EngineSpeedMomentDto, EngineSpeedMoment>();

            CreateMap<RecordDto, Record>();
            CreateMap<Record, RecordDto>();

            // For Cars
            CreateMap<CarDto, Car>();
            CreateMap<Car, CarDto>();

            CreateMap<Sensor, SensorForCarDto>();

            CreateMap<Subordinate, SubordinateForCarDto>();

            // For Sensors
            CreateMap<Sensor, SensorDto>();
            CreateMap<SensorDto, Sensor>();

            CreateMap<Car, CarForSensorDto>();

            // For Subordinate
            CreateMap<Subordinate, SubordinateDto>();
            CreateMap<SubordinateDto, Subordinate>();

            CreateMap<Car, CarForSubordinateDto>();

            // For User
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
