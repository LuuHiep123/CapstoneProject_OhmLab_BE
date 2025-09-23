using AutoMapper;
using BusinessLayer.RequestModel.RegistrationSchedule;
using BusinessLayer.ResponseModel.BaseResponse;
using BusinessLayer.ResponseModel.RegistrationSchedule;
using BusinessLayer.ResponseModel.Slot;
using BusinessLayer.ResponseModel.Schedule;
using BusinessLayer.ResponseModel.TeamKit;
using BusinessLayer.ResponseModel.User;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace BusinessLayer.Service.Implement
{
    public class RegistrationScheduleService : IRegistrationScheduleService
    {

        private readonly IRegistrationScheduleRepository _registrationScheduleRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ILabRepository _labRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClassUserRepository _classUserRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public RegistrationScheduleService(IClassUserRepository classUserRepository, IScheduleRepository scheduleRepository, ILabRepository labRepository, IUserRepository userRepository, ISlotRepository slotRepository, IClassRepository classRepository, IRegistrationScheduleRepository registrationScheduleRepository, IConfiguration configuration, IMapper mapper, IMemoryCache memoryCache)
        {
            _registrationScheduleRepository = registrationScheduleRepository;
            _labRepository = labRepository;
            _scheduleRepository = scheduleRepository;
            _classUserRepository = classUserRepository;
            _userRepository = userRepository;
            _slotRepository = slotRepository;
            _classRepository = classRepository;
            _configuration = configuration;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> AcceptRegistrtionSchedule(AcceptRejectRegistrationScheduleRequestModel model)
        {
            try
            {
                var registrationSchedule = await _registrationScheduleRepository.GetRegistrationScheduleById(model.RegistrationScheduleId);
                if (registrationSchedule == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found RegistrationSchedule!"

                    };
                }
                else
                {
                    if (!registrationSchedule.RegistrationScheduleStatus.ToLower().Equals("pending"))
                    {
                        return new BaseResponse<RegistrationScheduleAllResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "RegistrationSchedule has been processed!"

                        };
                    }

                    if (registrationSchedule.RegistrationScheduleStatus.ToLower().Equals("accept"))
                    {
                        return new BaseResponse<RegistrationScheduleAllResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "RegistrationSchedule has accept!"

                        };
                    }
                    registrationSchedule.RegistrationScheduleStatus = "Accept";
                    if (model.RegistrationScheduleNote != null)
                    {
                        registrationSchedule.RegistrationScheduleNote = model.RegistrationScheduleNote;
                    }
                    await _registrationScheduleRepository.UpdateRegistrationSchedule(registrationSchedule);
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Accept RegistrationSchedule success!",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> CheckDupplicateRegistrtionSchedule(CheckDupplicateRegitrationScheduleRequestModel model)
        {
            try
            {
                var listRegistrationSchedule = await _registrationScheduleRepository.GetAllRegistrationSchedule();
                var listCheck = listRegistrationSchedule.Where(rs => (rs.RegistrationScheduleDate == model.RegistrationScheduleDate && rs.SlotId == model.SlotId) && (!rs.RegistrationScheduleStatus.ToLower().Equals("reject")));

                if (listCheck.Any())
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = false,
                        Message = "Dupplicate RegistrationSchedule!"

                    };
                }
                else
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Not dupplicate RegistrationSchedule!",
                        Data = null
                    };
                }


            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> CreateRegistrationSchedule(CreateRegistrationScheduleRequestModel model)
        {
            try
            {
                var Class = await _classRepository.GetByIdAsync(model.ClassId);
                if (Class == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Class!"

                    };
                }

                var lab = await _labRepository.GetLabById(model.LabId);
                if (lab == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found lab!"

                    };
                }

                var slot = await _slotRepository.GetByIdAsync(model.SlotId);
                if (slot == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found slot!"

                    };
                }

                var teacher = await _userRepository.GetUserById(model.TeacherId);
                if (teacher == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Teacher!"

                    };
                }

                var registrationSchedule = _mapper.Map<RegistrationSchedule>(model);
                registrationSchedule.RegistrationScheduleStatus = "Pending";
                registrationSchedule.RegistrationScheduleCreateDate = DateTime.Now;

                await _registrationScheduleRepository.CreateRegistrationSchedule(registrationSchedule);
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = "Create RegistrationSchedule success!",
                    Data = null
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> DeleteRegistrationSchedule(int id)
        {
            try
            {
                var registrationSchedule = await _registrationScheduleRepository.GetRegistrationScheduleById(id);
                if(registrationSchedule == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found Teacher!"

                    };
                }
                else
                {
                    registrationSchedule.RegistrationScheduleStatus = "Delete";
                    await _registrationScheduleRepository.UpdateRegistrationSchedule(registrationSchedule);
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete RegistrationSchedule success!",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<DynamicResponse<RegistrationScheduleAllResponseModel>> GetAllRegistrationSchedule(GetAllRegistrationScheduleRequestModel model)
        {
            try
            {
                var listReschedule = await _registrationScheduleRepository.GetAllRegistrationSchedule();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<RegistrationSchedule> listRescheduleByTeacherRollNumber = listReschedule.Where(rs => rs.User.UserRollNumber.ToLower().Contains(model.keyWord.ToLower())).ToList();

                    List<RegistrationSchedule> listUserByTeacherName = listReschedule.Where(rs => rs.User.UserFullName.ToLower().Contains(model.keyWord.ToLower())).ToList();

                    listReschedule = listRescheduleByTeacherRollNumber
                               .Concat(listUserByTeacherName)
                               .GroupBy(rs => rs.RegistrationScheduleId)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.status))
                {
                    listReschedule = listReschedule.Where(rs => rs.RegistrationScheduleStatus.ToLower().Equals(model.status)).ToList();
                }
                var result = _mapper.Map<List<RegistrationScheduleAllResponseModel>>(listReschedule);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(rs => rs.RegistrationScheduleId) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<RegistrationScheduleAllResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedUsers.PageNumber,
                            Size = pagedUsers.PageSize,
                            Sort = "Ascending",
                            Order = "Id",
                            TotalPage = pagedUsers.PageCount,
                            TotalItem = pagedUsers.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.keyWord,
                            role = null,
                            status = model.status,
                        },
                        PageData = pagedUsers.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<List<RegistrationScheduleAllResponseModel>>> GetAllRegistrationScheduleByTeacherId(Guid teacherId)
        {
            try
            {
                var listReschedule = await _registrationScheduleRepository.GetAllRegistrationSchedule();

                var listRescheduleTeacher = listReschedule.Where(rs => rs.TeacherId.Equals(teacherId));
                var result = _mapper.Map<List<RegistrationScheduleAllResponseModel>>(listRescheduleTeacher);

                return new BaseResponse<List<RegistrationScheduleAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<RegistrationScheduleAllResponseModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> GetRegistrationScheduleById(int id)
        {
            try
            {
                var reschedule = await _registrationScheduleRepository.GetRegistrationScheduleById(id);
                if(reschedule == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found RegistrationSchedule!"

                    };
                }
                var result = _mapper.Map<RegistrationScheduleAllResponseModel>(reschedule);

                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<List<RegistrationScheduleAllResponseModel>>> GetRegistrationScheduleByStudentId(Guid studentId)
        {
            try
            {
                var listRegistrationSchedule = await _registrationScheduleRepository.GetAllRegistrationSchedule();
                var listClassUser = await _classUserRepository.GetByUserIdAsync(studentId);

                var filteredRegistrationSchedules = listRegistrationSchedule
                    .Where(s => listClassUser.Any(cu => cu.ClassId == s.ClassId))
                    .ToList();

                var result = _mapper.Map<List<RegistrationScheduleAllResponseModel>>(filteredRegistrationSchedules);
                return new BaseResponse<List<RegistrationScheduleAllResponseModel>>()
                {
                    Code = 200,
                    Success = true,
                    Message = "List RegisterSchedule by StudentId",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<RegistrationScheduleAllResponseModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<List<SlotResponseModel>>> GetSlotEmptyByDate(DateTime date)
        {

            try
            {
                var listSchedule = await _scheduleRepository.GetAllAsync();
                var listScheduleFilter = listSchedule.Where(s => s.ScheduleDate == date);

                var listRegistrationSchedule = await _registrationScheduleRepository.GetAllRegistrationSchedule();
                var listRegistrationScheduleFilter = listRegistrationSchedule.Where(rs => rs.RegistrationScheduleDate == date && rs.RegistrationScheduleStatus.ToLower().Equals("accept"));

                var listSlot = await _slotRepository.GetAllAsync();
                var listSlotDupplicate = new List<Slot>();
                if (listScheduleFilter.Any())
                {
                    foreach (var schedule in listScheduleFilter)
                    {
                        listSlotDupplicate.Add(schedule.Class.ScheduleType.Slot);
                    }
                }
                if (listRegistrationScheduleFilter.Any())
                {
                    foreach (var registrationSchedule in listRegistrationScheduleFilter)
                    {
                        listSlotDupplicate.Add(registrationSchedule.Slot);
                    }
                }
                if(listSlotDupplicate.Count() == listSlot.Count())
                {
                    return new BaseResponse<List<SlotResponseModel>>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Hết slot cho ngày đã chọn",
                        Data = null,

                    };
                }
                if (listSlotDupplicate.Any())
                {
                    var result = listSlot.Except(listSlotDupplicate).ToList();
                    var resultMap = _mapper.Map<List<SlotResponseModel>>(result);
                    return new BaseResponse<List<SlotResponseModel>>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = resultMap,

                    };
                }
                else
                {
                    var resultMap = _mapper.Map<List<SlotResponseModel>>(listSlot);
                    return new BaseResponse<List<SlotResponseModel>>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = resultMap,

                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<SlotResponseModel>>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> RejectRegistrtionSchedule(AcceptRejectRegistrationScheduleRequestModel model)
        {
            try
            {
                var registrationSchedule = await _registrationScheduleRepository.GetRegistrationScheduleById(model.RegistrationScheduleId);
                if (registrationSchedule == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found RegistrationSchedule!"

                    };
                }
                else
                {
                    if (!registrationSchedule.RegistrationScheduleStatus.ToLower().Equals("pending"))
                    {
                        return new BaseResponse<RegistrationScheduleAllResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "RegistrationSchedule has been processed!"

                        };
                    }
                    if (registrationSchedule.RegistrationScheduleStatus.ToLower().Equals("reject"))
                    {
                        return new BaseResponse<RegistrationScheduleAllResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "RegistrationSchedule has reject!"

                        };
                    }
                    registrationSchedule.RegistrationScheduleStatus = "Reject";
                    if (model.RegistrationScheduleNote != null)
                    {
                        registrationSchedule.RegistrationScheduleNote = model.RegistrationScheduleNote;
                    }
                    await _registrationScheduleRepository.UpdateRegistrationSchedule(registrationSchedule);
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Reject RegistrationSchedule success!",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<RegistrationScheduleAllResponseModel>> UpdateRegistrationSchedule(int id, UpdateRegistrationScheduleRequestModel model)
        {
            try
            {
                var registrationSchedule = await _registrationScheduleRepository.GetRegistrationScheduleById(id);
                if (registrationSchedule == null)
                {
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found RegistrationSchedule!"

                    };
                }
                else
                {
                    var result = _mapper.Map<RegistrationSchedule>(UpdateRegistrationSchedule);
                    result.RegistrationScheduleCreateDate = DateTime.Now;   
                    await _registrationScheduleRepository.UpdateRegistrationSchedule(result);
                    return new BaseResponse<RegistrationScheduleAllResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update RegistrationSchedule success!",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new BaseResponse<RegistrationScheduleAllResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
