using AutoMapper;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.Kit;
using BusinessLayer.ResponseModel.Schedule;
using BusinessLayer.ResponseModel.ScheduleType;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.Implement
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IClassUserRepository _classUserRepository;
        private readonly IMapper _mapper;

        public ScheduleService(IClassUserRepository classUserRepository, IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _classUserRepository = classUserRepository;
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleAsync()
        {
            try
            {
                var listSchedule = await _scheduleRepository.GetAllAsync();

                var result = _mapper.Map<List<ScheduleResponseAllModel>>(listSchedule);
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "List all schedule",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleByLectureIdAsync(Guid lectureId)
        {
            try
            {
                var listSchedule = await _scheduleRepository.GetAllAsync();

                listSchedule = listSchedule
                    .Where(s => s.Class.LecturerId.Equals(lectureId))
                    .ToList();

                var result = _mapper.Map<List<ScheduleResponseAllModel>>(listSchedule);
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "List schedule by LectureId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<List<ScheduleResponseAllModel>>> GetAllScheduleByStudentIdAsync(Guid userId)
        {
            try
            {
                var listSchedule = await _scheduleRepository.GetAllAsync();
                var listClassUser = await _classUserRepository.GetByUserIdAsync(userId);

                var filteredSchedules = listSchedule
                    .Where(s => listClassUser.Any(cu => cu.ClassId == s.ClassId))
                    .ToList();

                var result = _mapper.Map<List<ScheduleResponseAllModel>>(filteredSchedules);
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "List schedule by userId",
                    Data = result
                };  
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ScheduleResponseAllModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
