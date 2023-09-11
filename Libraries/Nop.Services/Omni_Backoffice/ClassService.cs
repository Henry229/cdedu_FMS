using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Services.Events;

namespace Nop.Services.Omni_Backoffice
{
    public partial class ClassService : IClassService
    {
        #region Constants

        private const string TEACHER_BY_ID_KEY = "Nop.teacher.id-{0}";
        private const string TEACHERS_ALL_KEY = "Nop.teacher.all";
        private const string TEACHERS_PATTERN_KEY = "Nop.teacher.";

        private const string TEACHERCAREER_BY_ID_KEY = "Nop.teachercareer.id-{0}";
        private const string TEACHERCAREERS_ALL_KEY = "Nop.teachercareer.all";
        private const string TEACHERCAREERS_PATTERN_KEY = "Nop.teachercareer.";

        private const string EVALUATION_BY_ID_KEY = "Nop.evaluation.id-{0}";
        private const string EVALUATIONS_ALL_KEY = "Nop.evaluation.all";
        private const string EVALUATIONS_PATTERN_KEY = "Nop.evaluation.";

        private const string TEACHERSUBJECT_BY_ID_KEY = "Nop.teachersubject.id-{0}";
        private const string TEACHERSUBJECTS_ALL_KEY = "Nop.teachersubject.all";
        private const string TEACHERSUBJECTS_PATTERN_KEY = "Nop.teachersubject.";

        private const string TEACHERBRANCH_BY_ID_KEY = "Nop.teacherbranch.id-{0}";
        private const string TEACHERBRANCHS_ALL_KEY = "Nop.teacherbranch.all";
        private const string TEACHERBRANCHS_PATTERN_KEY = "Nop.teacherbranch.";

        private const string CLASSROOM_BY_ID_KEY = "Nop.classroom.id-{0}";
        private const string CLASSROOMS_ALL_KEY = "Nop.classroom.all";
        private const string CLASSROOMS_PATTERN_KEY = "Nop.classroom.";

        private const string CLASSINFO_BY_ID_KEY = "Nop.class.id-{0}";
        private const string CLASSINFOS_ALL_KEY = "Nop.class.all";
        private const string CLASSINFOS_PATTERN_KEY = "Nop.class.";

        private const string CLASSTEACHER_BY_ID_KEY = "Nop.classteacher.id-{0}";
        private const string CLASSTEACHERS_ALL_KEY = "Nop.classteacher.all";
        private const string CLASSTEACHERS_PATTERN_KEY = "Nop.classteacher.";

        private const string CLASSENROL_BY_ID_KEY = "Nop.classenrol.id-{0}";
        private const string CLASSENROLS_ALL_KEY = "Nop.classenrol.all";
        private const string CLASSENROLS_PATTERN_KEY = "Nop.classenrol.";

        private const string CLASSENROLPAY_BY_ID_KEY = "Nop.classenrolpay.id-{0}";
        private const string CLASSENROLPAYS_ALL_KEY = "Nop.classenrolpay.all";
        private const string CLASSENROLPAYS_PATTERN_KEY = "Nop.classenrolpay.";

        private const string CLASSSCHEDULE_BY_ID_KEY = "Nop.classschedule.id-{0}";
        private const string CLASSSCHEDULES_ALL_KEY = "Nop.classschedule.all";
        private const string CLASSSCHEDULES_PATTERN_KEY = "Nop.classschedule.";

        private const string CLASSSCHEDULETEACHER_BY_ID_KEY = "Nop.classscheduleteacher.id-{0}";
        private const string CLASSSCHEDULETEACHERS_ALL_KEY = "Nop.classscheduleteacher.all";
        private const string CLASSSCHEDULETEACHERS_PATTERN_KEY = "Nop.classscheduleteacher.";

        private const string CLASSSCHEDULEROLLCALL_BY_ID_KEY = "Nop.classschedulerollcall.id-{0}";
        private const string CLASSSCHEDULEROLLCALLS_ALL_KEY = "Nop.classschedulerollcall.all";
        private const string CLASSSCHEDULEROLLCALLS_PATTERN_KEY = "Nop.classschedulerollcall.";

        private const string PARENT_BY_ID_KEY = "Nop.parent.id-{0}";
        private const string PARENTS_ALL_KEY = "Nop.parent.all";
        private const string PARENTS_PATTERN_KEY = "Nop.parent.";

        private const string SIBLINGMANAGE_BY_ID_KEY = "Nop.siblingmanage.id-{0}";
        private const string SIBLINGMANAGES_ALL_KEY = "Nop.siblingmanage.all";
        private const string SIBLINGMANAGES_PATTERN_KEY = "Nop.siblingmanage.";

        private const string ADDITIONINFO_BY_ID_KEY = "Nop.additioninfo.id-{0}";
        private const string ADDITIONINFOS_ALL_KEY = "Nop.additioninfo.all";
        private const string ADDITIONINFOS_PATTERN_KEY = "Nop.additioninfo.";

        private const string STUDENTBRANCH_BY_ID_KEY = "Nop.studentbranch.id-{0}";
        private const string STUDENTBRANCHS_ALL_KEY = "Nop.studentbranch.all";
        private const string STUDENTBRANCHS_PATTERN_KEY = "Nop.studentbranch.";


        #endregion


        #region Fields
        private readonly IRepository<Teacher> _teacherRepository;
        private readonly IRepository<TeacherCareer> _teachercareerRepository;
        private readonly IRepository<Evaluation> _evaluationRepository;
        private readonly IRepository<TeacherSubject> _teachersubjectRepository;
        private readonly IRepository<TeacherBranch> _teacherbranchRepository;
        private readonly IRepository<ClassRoom> _classroomRepository;
        private readonly IRepository<ClassInfo> _classinfoRepository;
        private readonly IRepository<ClassTeacher> _classteacherRepository;
        private readonly IRepository<ClassEnrol> _classenrolRepository;
        private readonly IRepository<ClassEnrol_Pay> _classenrolpayRepository;
        private readonly IRepository<Member> _memberRepository;
        private readonly IRepository<ClassSchedule> _classscheduleRepository;
        private readonly IRepository<ClassScheduleTeacher> _classscheduleteacherRepository;
        private readonly IRepository<ClassScheduleRollcall> _classschedulerollcallRepository;
        private readonly IRepository<Parent> _parentRepository;
        private readonly IRepository<SiblingManage> _siblingmanageRepository;
        private readonly IRepository<AdditionInfo> _additioninfoRepository;
        private readonly IRepository<StudentBranch> _studentbranchRepository;

        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        #endregion


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="itemRepository">item repository</param>
        public ClassService(ICacheManager cacheManager, IEventPublisher eventPublisher,
            IRepository<Teacher> teacherRepository, IRepository<TeacherCareer> teachercareerRepository
            , IRepository<Evaluation> evaluationRepository, IRepository<TeacherSubject> teachersubjectRepository, IRepository<TeacherBranch> teacherbranchRepository
            , IRepository<ClassRoom> classroomRepository, IRepository<ClassInfo> classinfoRepository, IRepository<ClassTeacher> classteacherRepository
            , IRepository<ClassEnrol> classenrolRepository, IRepository<Member> memberRepository, IRepository<ClassEnrol_Pay> classenrolpayRepository
            , IRepository<ClassSchedule> classscheduleRepository, IRepository<ClassScheduleTeacher> classscheduleteacherRepository
            , IRepository<ClassScheduleRollcall> classschedulerollcallRepository, IRepository<Parent> parentRepository
            , IRepository<SiblingManage> siblingmanageRepository, IRepository<AdditionInfo> additioninfoRepository, IRepository<StudentBranch> studentbranchRepository)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._teacherRepository = teacherRepository;
            this._teachercareerRepository = teachercareerRepository;
            this._evaluationRepository = evaluationRepository;
            this._teachersubjectRepository = teachersubjectRepository;
            this._teacherbranchRepository = teacherbranchRepository;
            this._classroomRepository = classroomRepository;
            this._classinfoRepository = classinfoRepository;
            this._classteacherRepository = classteacherRepository;
            this._classenrolRepository = classenrolRepository;
            this._memberRepository = memberRepository;
            this._classenrolpayRepository = classenrolpayRepository;
            this._classscheduleRepository = classscheduleRepository;
            this._classscheduleteacherRepository = classscheduleteacherRepository;
            this._classschedulerollcallRepository = classschedulerollcallRepository;
            this._parentRepository = parentRepository;
            this._siblingmanageRepository = siblingmanageRepository;
            this._additioninfoRepository = additioninfoRepository;
            this._studentbranchRepository = studentbranchRepository;
        }

        #endregion



        #region Teacher

        public virtual Teacher GetTeacherById(int teacherID)
        {
            if (teacherID == 0)
                return null;

            return _teacherRepository.GetById(teacherID);
        }


        public virtual void InsertTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException("teacher");

            _teacherRepository.Insert(teacher);

            //cache

            _cacheManager.RemoveByPattern(TEACHERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(teacher);
        }


        public virtual void UpdateTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException("teacher");

            _teacherRepository.Update(teacher);

            //cache
            _cacheManager.RemoveByPattern(TEACHERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(teacher);
        }


        public virtual void DeleteTeacher(Teacher teacher)
        {
            if (teacher == null)
                throw new ArgumentNullException("teacher");

            _teacherRepository.Delete(teacher);

            //cache
            _cacheManager.RemoveByPattern(TEACHERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(teacher);
        }

        public virtual IPagedList<Teacher> GetAllTeachers(string Branch, string FirstName, string LastName, string Subject, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _teacherRepository.Table;

            if (!String.IsNullOrEmpty(Branch))
                query = query.Where(x => x.Branch == Branch);

            if (!String.IsNullOrEmpty(FirstName))
                query = query.Where(x => x.FirstName == FirstName);

            if (!String.IsNullOrEmpty(LastName))
                query = query.Where(x => x.LastName == LastName);



            query = query.OrderBy(c => c.FirstName).ThenBy( c => c.LastName);

            try
            {
                var pageditems = new PagedList<Teacher>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion





        #region TeacherCareer
        
        public virtual TeacherCareer GetTeacherCareerById(int teachercareerID)
        {
            if (teachercareerID == 0)
                return null;

            return _teachercareerRepository.GetById(teachercareerID);
        }


        public virtual void InsertTeacherCareer(TeacherCareer teachercareer)
        {
            if (teachercareer == null)
                throw new ArgumentNullException("teachercareer");

            _teachercareerRepository.Insert(teachercareer);

            //cache

            _cacheManager.RemoveByPattern(TEACHERCAREERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(teachercareer);
        }


        public virtual void UpdateTeacherCareer(TeacherCareer teachercareer)
        {
            if (teachercareer == null)
                throw new ArgumentNullException("teachercareer");

            _teachercareerRepository.Update(teachercareer);

            //cache
            _cacheManager.RemoveByPattern(TEACHERCAREERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(teachercareer);
        }


        public virtual void DeleteTeacherCareer(TeacherCareer teachercareer)
        {
            if (teachercareer == null)
                throw new ArgumentNullException("teachercareer");

            _teachercareerRepository.Delete(teachercareer);

            //cache
            _cacheManager.RemoveByPattern(TEACHERCAREERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(teachercareer);
        }

        public virtual IPagedList<TeacherCareer> GetAllTeacherCareers(int teacherid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _teachercareerRepository.Table;
            query = query.Where(c => c.Teacher_Id == teacherid);
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var pageditems = new PagedList<TeacherCareer>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion



        #region TeacherSubject

        public virtual TeacherSubject GetTeacherSubjectById(int teachersubjectID)
        {
            if (teachersubjectID == 0)
                return null;

            return _teachersubjectRepository.GetById(teachersubjectID);
        }


        public virtual void InsertTeacherSubject(TeacherSubject teachersubject)
        {
            if (teachersubject == null)
                throw new ArgumentNullException("teachersubject");

            _teachersubjectRepository.Insert(teachersubject);

            //cache

            _cacheManager.RemoveByPattern(TEACHERSUBJECTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(teachersubject);
        }


        public virtual void UpdateTeacherSubject(TeacherSubject teachersubject)
        {
            if (teachersubject == null)
                throw new ArgumentNullException("teachersubject");

            _teachersubjectRepository.Update(teachersubject);

            //cache
            _cacheManager.RemoveByPattern(TEACHERSUBJECTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(teachersubject);
        }


        public virtual void DeleteTeacherSubject(TeacherSubject teachersubject)
        {
            if (teachersubject == null)
                throw new ArgumentNullException("teachersubject");

            _teachersubjectRepository.Delete(teachersubject);

            //cache
            _cacheManager.RemoveByPattern(TEACHERSUBJECTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(teachersubject);
        }

        public virtual IPagedList<TeacherSubject> GetAllTeacherSubjects(int teacherid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _teachersubjectRepository.Table;
            query = query.Where(c => c.Teacher_Id == teacherid);
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var pageditems = new PagedList<TeacherSubject>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }

        public virtual IPagedList<TeacherSubject> GetAllClassTeacherSubjects(int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _teachersubjectRepository.Table;
            //query = query.Where(c => c.Teacher_Id == teacherid);
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var pageditems = new PagedList<TeacherSubject>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }
        #endregion






        #region TeacherBranch

        public virtual TeacherBranch GetTeacherBranchById(int teacherbranchID)
        {
            if (teacherbranchID == 0)
                return null;

            return _teacherbranchRepository.GetById(teacherbranchID);
        }


        public virtual void InsertTeacherBranch(TeacherBranch teacherbranch)
        {
            if (teacherbranch == null)
                throw new ArgumentNullException("teacherbranch");

            _teacherbranchRepository.Insert(teacherbranch);

            //cache

            _cacheManager.RemoveByPattern(TEACHERBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(teacherbranch);
        }


        public virtual void UpdateTeacherBranch(TeacherBranch teacherbranch)
        {
            if (teacherbranch == null)
                throw new ArgumentNullException("teacherbranch");

            _teacherbranchRepository.Update(teacherbranch);

            //cache
            _cacheManager.RemoveByPattern(TEACHERBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(teacherbranch);
        }


        public virtual void DeleteTeacherBranch(TeacherBranch teacherbranch)
        {
            if (teacherbranch == null)
                throw new ArgumentNullException("teacherbranch");

            _teacherbranchRepository.Delete(teacherbranch);

            //cache
            _cacheManager.RemoveByPattern(TEACHERBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(teacherbranch);
        }

        public virtual IPagedList<TeacherBranch> GetAllTeacherBranchs(int teacherid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _teacherbranchRepository.Table;
            query = query.Where(c => c.Teacher_Id == teacherid);
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var pageditems = new PagedList<TeacherBranch>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion





        #region Evaluation

        public virtual Evaluation GetEvaluationById(int evaluationID)
        {
            if (evaluationID == 0)
                return null;

            return _evaluationRepository.GetById(evaluationID);
        }


        public virtual void InsertEvaluation(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException("evaluation");

            _evaluationRepository.Insert(evaluation);

            //cache

            _cacheManager.RemoveByPattern(EVALUATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(evaluation);
        }


        public virtual void UpdateEvaluation(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException("evaluation");

            _evaluationRepository.Update(evaluation);

            //cache
            _cacheManager.RemoveByPattern(EVALUATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(evaluation);
        }


        public virtual void DeleteEvaluation(Evaluation evaluation)
        {
            if (evaluation == null)
                throw new ArgumentNullException("evaluation");

            _evaluationRepository.Delete(evaluation);

            //cache
            _cacheManager.RemoveByPattern(EVALUATIONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(evaluation);
        }

        public virtual IPagedList<Evaluation> GetAllEvaluations(int teacherid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _evaluationRepository.Table;
            query = query.Where(c => c.Teacher_Id == teacherid);

            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var pageditems = new PagedList<Evaluation>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion


        #region ClassRoom

        public virtual ClassRoom GetClassRoomById(int classroomID)
        {
            if (classroomID == 0)
                return null;

            return _classroomRepository.GetById(classroomID);
        }


        public virtual void InsertClassRoom(ClassRoom classroom)
        {
            if (classroom == null)
                throw new ArgumentNullException("classroom");

            _classroomRepository.Insert(classroom);

            //cache

            _cacheManager.RemoveByPattern(CLASSROOMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(classroom);
        }


        public virtual void UpdateClassRoom(ClassRoom classroom)
        {
            if (classroom == null)
                throw new ArgumentNullException("classroom");

            _classroomRepository.Update(classroom);

            //cache
            _cacheManager.RemoveByPattern(CLASSROOMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classroom);
        }


        public virtual void DeleteClassRoom(ClassRoom classroom)
        {
            if (classroom == null)
                throw new ArgumentNullException("classroom");

            _classroomRepository.Delete(classroom);

            //cache
            _cacheManager.RemoveByPattern(CLASSROOMS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(classroom);
        }

        public virtual IPagedList<ClassRoom> GetAllClassRooms(string branch, string title, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classroomRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.Branch == branch);
            }
            if (title != null && title != "")
            {
                query = query.Where(x => x.Title == title);
            }
            query = query.OrderBy(c => c.Title);

            try
            {
                var pageditems = new PagedList<ClassRoom>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion



        #region ClassInfo

        public virtual void InsertClassInfo(ClassInfo classinfo)
        {
            if (classinfo == null)
                throw new ArgumentNullException("classinfo");

            _classinfoRepository.Insert(classinfo);

            //cache

            _cacheManager.RemoveByPattern(CLASSINFOS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(classinfo);
        }


        public virtual void UpdateClassInfo(ClassInfo classinfo)
        {
            if (classinfo == null)
                throw new ArgumentNullException("classinfo");

            _classinfoRepository.Update(classinfo);

            //cache

            _cacheManager.RemoveByPattern(CLASSINFOS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classinfo);
        }

        public virtual void DeleteClassInfo(ClassInfo classinfo)
        {
            if (classinfo == null)
                throw new ArgumentNullException("classinfo");

            _classinfoRepository.Delete(classinfo);

            //cache
            _cacheManager.RemoveByPattern(CLASSINFOS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(classinfo);
        }


        /// <summary>
        /// Gets a class by identifier
        /// </summary>
        /// <returns>Class</returns>
        /// 
        public virtual ClassInfo GetClassInfoById(int classinfoId)
        {
            if (classinfoId == 0)
                return null;

            return _classinfoRepository.GetById(classinfoId);
        }

        /// <summary>
        /// Gets a class by identifier
        /// </summary>
        /// <returns>Class</returns>
        /// 
        //public virtual ClassInfo GetClassByCode(string itemcode)
        //{
        //    if (itemcode == "")
        //        return null;

        //    var item = _itemRepository.Table.Where(s => s.ItemCode == itemcode);
        //    var itemlist = item.ToList();
        //    if (itemlist.Count != 1)
        //        return null;

        //    return itemlist[0];
        //}


        //public bool CheckDup(Item item)
        //{
        //    var same = _itemRepository.Table.Where(s => s.ItemCode == item.ItemCode);
        //    if (same.ToList().Count > 0)
        //        return true;
        //    return false;
        //}

        //public string GenerateItemCode(string category)
        //{
        //    if (category == null || category == "" || category.Length != 4)
        //        return "";

        //    var list = _itemRepository.Table.Where(s => s.ItemCode.StartsWith(category)).OrderByDescending(s => s.ItemCode).ToList();

        //    int seq = 1;
        //    if (list.Count != 0)
        //    {
        //        seq = Int32.Parse(list[0].ItemCode.Substring(4, 6));
        //        seq = seq + 1;
        //    }

        //    return category + seq.ToString("000000");
        //}


        /// <summary>
        /// Gets all classes
        /// </summary>
        /// <returns>class collection</returns>
        public virtual IPagedList<ClassInfo> GetAllClassInfos(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classinfoRepository.Table;

           
            if (!String.IsNullOrWhiteSpace(branch))
            {
                query = query.Where(c => c.Branch.Contains(branch));
            }
            if (!String.IsNullOrWhiteSpace(term))
            {
                query = query.Where(c => c.Term.Contains(term));
            }
            if (!String.IsNullOrWhiteSpace(year))
            {
                query = query.Where(c => c.Year.Contains(year));
            }


            query = query.OrderBy(c => c.Year);
            try
            {
                var pageditems = new PagedList<ClassInfo>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
            
        }

        #endregion

        #region ClassTeacher

        public ClassTeacher GetClassTeacherById(int classId)
        {
            if (classId == 0)
                return null;

            return _classteacherRepository.GetById(classId);
        }

        public void InsertClassTeacher(ClassTeacher classteacher)
        {
            if (classteacher == null)
                throw new ArgumentNullException("classteacher");

            try
            {
                _classteacherRepository.Insert(classteacher);

                //cache

                _cacheManager.RemoveByPattern(CLASSTEACHERS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classteacher);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassTeacher(ClassTeacher classteacher)
        {
            if (classteacher == null)
                throw new ArgumentNullException("classteacher");

            _classteacherRepository.Delete(classteacher);

            //cache
            _cacheManager.RemoveByPattern(CLASSTEACHERS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classteacher);
        }

        public void UpdateClassTeacher(ClassTeacher classteacher)
        {
            if (classteacher == null)
                throw new ArgumentNullException("classteacher");

            _classteacherRepository.Update(classteacher);

            //cache

            _cacheManager.RemoveByPattern(CLASSTEACHERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classteacher);
        }

        public IPagedList<ClassTeacher> GetAllClassTeachers(int classid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classteacherRepository.Table;
            //if (classid != null && classid != "")
            //{
                query = query.Where(c => c.Class_Id == classid);
            //}
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var classteachers = new PagedList<ClassTeacher>(query, pageIndex, pageSize);
                return classteachers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region ClassEnrol

        public ClassEnrol GetClassEnrolById(int classId)
        {
            if (classId == 0)
                return null;

            return _classenrolRepository.GetById(classId);
        }

        public void InsertClassEnrol(ClassEnrol classenrol)
        {
            if (classenrol == null)
                throw new ArgumentNullException("classenrol");

            try
            {
                _classenrolRepository.Insert(classenrol);

                //cache

                _cacheManager.RemoveByPattern(CLASSENROLS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classenrol);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassEnrol(ClassEnrol classenrol)
        {
            if (classenrol == null)
                throw new ArgumentNullException("classenrol");

            _classenrolRepository.Delete(classenrol);

            //cache
            _cacheManager.RemoveByPattern(CLASSENROLS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classenrol);
        }

        public void UpdateClassEnrol(ClassEnrol classenrol)
        {
            if (classenrol == null)
                throw new ArgumentNullException("classenrol");

            _classenrolRepository.Update(classenrol);

            //cache

            _cacheManager.RemoveByPattern(CLASSENROLS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classenrol);
        }

        public IPagedList<ClassEnrol> GetAllClassEnrols(int classid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classenrolRepository.Table;
            if (classid != null && classid != 0)
            {
            query = query.Where(c => c.Class_Id == classid);
            }
            query = query.OrderBy(c => c.Stud_Id);

            try
            {
                var classenrols = new PagedList<ClassEnrol>(query, pageIndex, pageSize);
                return classenrols;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region ClassEnrol_Pay

        public ClassEnrol_Pay GetClassEnrol_PayById(int classenrolpayId)
        {
            if (classenrolpayId == 0)
                return null;

            return _classenrolpayRepository.GetById(classenrolpayId);
        }

        public void InsertClassEnrol_Pay(ClassEnrol_Pay classenrolpay)
        {
            if (classenrolpay == null)
                throw new ArgumentNullException("classenrolpay");

            try
            {
                _classenrolpayRepository.Insert(classenrolpay);

                //cache

                _cacheManager.RemoveByPattern(CLASSENROLPAYS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classenrolpay);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassEnrol_Pay(ClassEnrol_Pay classenrolpay)
        {
            if (classenrolpay == null)
                throw new ArgumentNullException("classenrolpay");

            _classenrolpayRepository.Delete(classenrolpay);

            //cache
            _cacheManager.RemoveByPattern(CLASSENROLPAYS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classenrolpay);
        }

        public void UpdateClassEnrol_Pay(ClassEnrol_Pay classenrolpay)
        {
            if (classenrolpay == null)
                throw new ArgumentNullException("classenrolpay");

            _classenrolpayRepository.Update(classenrolpay);

            //cache

            _cacheManager.RemoveByPattern(CLASSENROLS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classenrolpay);
        }

        public IPagedList<ClassEnrol_Pay> GetAllClassEnrol_Pays(int classenrolid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classenrolpayRepository.Table;
            if (classenrolid != null && classenrolid != 0)
            {
                query = query.Where(c => c.Id_Enrol == classenrolid);
            }
            query = query.OrderBy(c => c.Seq);

            try
            {
                var classenrols = new PagedList<ClassEnrol_Pay>(query, pageIndex, pageSize);
                return classenrols;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region member
       
        public IPagedList<Member> GetAllMembers(string branch, string grade, string firstname, string lastname, string stud_id, string id_number, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _memberRepository.Table; 

            if ( !String.IsNullOrEmpty(branch))
            {
                query = query.Where(c => c.branch == branch);
            }

            if ( !String.IsNullOrEmpty(grade) )
            {
                var yeargrade = Int32.Parse(grade.Replace("YR", "")).ToString();
                query = query.Where(c => c.grade == yeargrade);
            }

            if (!String.IsNullOrEmpty(firstname))
            {
                query = query.Where(c => c.stud_first_name.Contains(firstname));
            }

            if (!String.IsNullOrEmpty(lastname))
            {
                query = query.Where(c => c.stud_last_name.Contains(lastname));
            }

            if (!String.IsNullOrEmpty(stud_id))
            {
                query = query.Where(c => c.stud_id == stud_id);
            }

            if (!String.IsNullOrEmpty(id_number))
            {
                query = query.Where(c => c.id_number == id_number);
            }

            query = query.Where( c => !String.IsNullOrEmpty(c.id_number));

            query = query.OrderBy(c => c.stud_first_name);


            try
            {
                var members = new PagedList<Member>(query, pageIndex, pageSize);
                return members;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region ClassSchedule

        public ClassSchedule GetClassScheduleById(int classId)
        {
            if (classId == 0)
                return null;

            return _classscheduleRepository.GetById(classId);
        }

        public void InsertClassSchedule(ClassSchedule classschedule)
        {
            if (classschedule == null)
                throw new ArgumentNullException("classschedule");

            try
            {
                _classscheduleRepository.Insert(classschedule);

                //cache

                _cacheManager.RemoveByPattern(CLASSSCHEDULES_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classschedule);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassSchedule(ClassSchedule classschedule)
        {
            if (classschedule == null)
                throw new ArgumentNullException("classschedule");

            _classscheduleRepository.Delete(classschedule);

            //cache
            _cacheManager.RemoveByPattern(CLASSSCHEDULES_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classschedule);
        }

        public void UpdateClassSchedule(ClassSchedule classschedule)
        {
            if (classschedule == null)
                throw new ArgumentNullException("classschedule");

            _classscheduleRepository.Update(classschedule);

            //cache

            _cacheManager.RemoveByPattern(CLASSSCHEDULES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classschedule);
        }

        public IPagedList<ClassSchedule> GetAllClassSchedules(int classid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classscheduleRepository.Table;
            if (classid != null && classid != 0)
            {
                query = query.Where(c => c.Class_Id == classid);
            }
            query = query.OrderBy(c => c.Class_Id);

            try
            {
                var classschedules = new PagedList<ClassSchedule>(query, pageIndex, pageSize);
                return classschedules;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #endregion

        #region ClassScheduleTeacher

        public ClassScheduleTeacher GetClassScheduleTeacherById(int classId)
        {
            if (classId == 0)
                return null;

            return _classscheduleteacherRepository.GetById(classId);
        }

        public void InsertClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher)
        {
            if (classscheduleteacher == null)
                throw new ArgumentNullException("classscheduleteacher");

            try
            {
                _classscheduleteacherRepository.Insert(classscheduleteacher);

                //cache

                _cacheManager.RemoveByPattern(CLASSSCHEDULETEACHERS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classscheduleteacher);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher)
        {
            if (classscheduleteacher == null)
                throw new ArgumentNullException("classscheduleteacher");

            _classscheduleteacherRepository.Delete(classscheduleteacher);

            //cache
            _cacheManager.RemoveByPattern(CLASSSCHEDULETEACHERS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classscheduleteacher);
        }

        public void UpdateClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher)
        {
            if (classscheduleteacher == null)
                throw new ArgumentNullException("classscheduleteacher");

            _classscheduleteacherRepository.Update(classscheduleteacher);

            //cache

            _cacheManager.RemoveByPattern(CLASSSCHEDULETEACHERS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classscheduleteacher);
        }

        public IPagedList<ClassScheduleTeacher> GetAllClassScheduleTeachers(int classid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classscheduleteacherRepository.Table;
            //if (classid != null && classid != "")
            //{
            query = query.Where(c => c.Class_D_Id == classid);
            //}
            query = query.OrderBy(c => c.Teacher_Id);

            try
            {
                var classscheduleteachers = new PagedList<ClassScheduleTeacher>(query, pageIndex, pageSize);
                return classscheduleteachers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region ClassScheduleRollcall

        public ClassScheduleRollcall GetClassScheduleRollcallById(int classId)
        {
            if (classId == 0)
                return null;

            return _classschedulerollcallRepository.GetById(classId);
        }

        public void InsertClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall)
        {
            if (classschedulerollcall == null)
                throw new ArgumentNullException("classschedulerollcall");

            try
            {
                _classschedulerollcallRepository.Insert(classschedulerollcall);

                //cache

                _cacheManager.RemoveByPattern(CLASSSCHEDULEROLLCALLS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(classschedulerollcall);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall)
        {
            if (classschedulerollcall == null)
                throw new ArgumentNullException("classschedulerollcall");

            _classschedulerollcallRepository.Delete(classschedulerollcall);

            //cache
            _cacheManager.RemoveByPattern(CLASSSCHEDULEROLLCALLS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(classschedulerollcall);
        }

        public void UpdateClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall)
        {
            if (classschedulerollcall == null)
                throw new ArgumentNullException("classschedulerollcall");

            _classschedulerollcallRepository.Update(classschedulerollcall);

            //cache

            _cacheManager.RemoveByPattern(CLASSSCHEDULEROLLCALLS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(classschedulerollcall);
        }

        public IPagedList<ClassScheduleRollcall> GetAllClassScheduleRollcalls(int classid, int classdid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _classschedulerollcallRepository.Table;
            if (classid != null && classid != 0)
            {
            query = query.Where(c => c.Class_Id == classid) ;
            
            }

            if (classdid != null && classdid != 0)
            {
                query = query.Where(c => c.Class_D_Id == classdid);

            }

            query = query.OrderBy(c => c.Class_D_Id);

            try
            {
                var classschedulerollcalls = new PagedList<ClassScheduleRollcall>(query, pageIndex, pageSize);
                return classschedulerollcalls;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region Parent

        public virtual Parent GetParentById(int parentID)
        {
            if (parentID == 0)
                return null;

            return _parentRepository.GetById(parentID);
        }


        public virtual void InsertParent(Parent parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parentRepository.Insert(parent);

            //cache

            _cacheManager.RemoveByPattern(PARENTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(parent);
        }


        public virtual void UpdateParent(Parent parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parentRepository.Update(parent);

            //cache
            _cacheManager.RemoveByPattern(PARENTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(parent);
        }


        public virtual void DeleteParent(Parent parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parentRepository.Delete(parent);

            //cache
            _cacheManager.RemoveByPattern(PARENTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(parent);
        }

        public virtual IPagedList<Parent> GetAllParents(string branch, string firstname, string lastname, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _parentRepository.Table;
            if (branch != null && branch != "")
            {
                query = query.Where(x => x.Branch == branch);
            }
            if (firstname != null && firstname != "")
            {
                query = query.Where(x => x.FirstName == firstname);
            }
            if (lastname != null && lastname != "")
            {
                query = query.Where(x => x.LastName == lastname);
            }
            query = query.OrderBy(c => c.FirstName);

            try
            {
                var pageditems = new PagedList<Parent>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion

        
        #region SiblingManage

        public virtual SiblingManage GetSiblingManageById(int siblingmanageID)
        {
            if (siblingmanageID == 0)
                return null;

            return _siblingmanageRepository.GetById(siblingmanageID);
        }


        public virtual void InsertSiblingManage(SiblingManage siblingmanage)
        {
            if (siblingmanage == null)
                throw new ArgumentNullException("siblingmanage");

            _siblingmanageRepository.Insert(siblingmanage);

            //cache

            _cacheManager.RemoveByPattern(SIBLINGMANAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(siblingmanage);
        }


        public virtual void UpdateSiblingManage(SiblingManage siblingmanage)
        {
            if (siblingmanage == null)
                throw new ArgumentNullException("siblingmanage");

            _siblingmanageRepository.Update(siblingmanage);

            //cache
            _cacheManager.RemoveByPattern(SIBLINGMANAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(siblingmanage);
        }


        public virtual void DeleteSiblingManage(SiblingManage siblingmanage)
        {
            if (siblingmanage == null)
                throw new ArgumentNullException("siblingmanage");

            _siblingmanageRepository.Delete(siblingmanage);

            //cache
            _cacheManager.RemoveByPattern(SIBLINGMANAGES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(siblingmanage);
        }

        public virtual IPagedList<SiblingManage> GetAllSiblingManages(int parent_id,int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _siblingmanageRepository.Table;
            if (parent_id != null && parent_id != 0)
            {
                query = query.Where(x => x.Parent_Id == parent_id);
            }
           
            query = query.OrderBy(c => c.Seq);

            try
            {
                var pageditems = new PagedList<SiblingManage>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion


        #region Addition Info

        public AdditionInfo GetAdditionInfoById(int additioninfoId)
        {
            if (additioninfoId == 0)
                return null;

            return _additioninfoRepository.GetById(additioninfoId);
        }

        public void InsertAdditionInfo(AdditionInfo additioninfo)
        {
            if (additioninfo == null)
                throw new ArgumentNullException("additioninfo");

            try
            {
                _additioninfoRepository.Insert(additioninfo);

                //cache

                _cacheManager.RemoveByPattern(ADDITIONINFOS_PATTERN_KEY);

                //event notification
                _eventPublisher.EntityInserted(additioninfo);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }
        }

        public void DeleteAdditionInfo(AdditionInfo additioninfo)
        {
            if (additioninfo == null)
                throw new ArgumentNullException("additioninfo");

            _additioninfoRepository.Delete(additioninfo);

            //cache
            _cacheManager.RemoveByPattern(ADDITIONINFOS_PATTERN_KEY);


            //event notification
            _eventPublisher.EntityDeleted(additioninfo);
        }

        public void UpdateAdditionInfo(AdditionInfo additioninfo)
        {
            if (additioninfo == null)
                throw new ArgumentNullException("additioninfo");

            _additioninfoRepository.Update(additioninfo);

            //cache

            _cacheManager.RemoveByPattern(ADDITIONINFOS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(additioninfo);
        }

        public IPagedList<AdditionInfo> GetAllAdditionInfos(string studid, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _additioninfoRepository.Table;
            if (studid != null && studid != "")
            {
                query = query.Where(c => c.Stud_Id == studid);
            }
            query = query.OrderBy(c => c.Stud_Id);

            try
            {
                var additioninfos = new PagedList<AdditionInfo>(query, pageIndex, pageSize);
                return additioninfos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region StudentBranch

        public virtual StudentBranch GetStudentBranchById(int studentbranchId)
        {
            if (studentbranchId == 0)
                return null;

            return _studentbranchRepository.GetById(studentbranchId);
        }


        public virtual void InsertStudentBranch(StudentBranch studentbranch)
        {
            if (studentbranch == null)
                throw new ArgumentNullException("studentbranch");

            _studentbranchRepository.Insert(studentbranch);

            //cache

            _cacheManager.RemoveByPattern(STUDENTBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(studentbranch);
        }


        public virtual void UpdateStudentBranch(StudentBranch studentbranch)
        {
            if (studentbranch == null)
                throw new ArgumentNullException("studentbranch");

            _studentbranchRepository.Update(studentbranch);

            //cache
            _cacheManager.RemoveByPattern(STUDENTBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(studentbranch);
        }


        public virtual void DeleteStudentBranch(StudentBranch studentbranch)
        {
            if (studentbranch == null)
                throw new ArgumentNullException("studentbranch");

            _studentbranchRepository.Delete(studentbranch);

            //cache
            _cacheManager.RemoveByPattern(STUDENTBRANCHS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(studentbranch);
        }

        public virtual IPagedList<StudentBranch> GetAllStudentBranchs(string studid, string branch, int pageIndex = 0, int pageSize = 2147483647)
        {
            var query = _studentbranchRepository.Table;

            query = query.Where(c => c.Stud_Id == studid);
            if (!String.IsNullOrWhiteSpace(branch))
            {
                query = query.Where(c => c.Branch.Contains(branch));
            }
            query = query.OrderBy(c => c.Stud_Id);

            try
            {
                var pageditems = new PagedList<StudentBranch>(query, pageIndex, pageSize);
                return pageditems;
            }
            catch (Exception ex)
            {
                string stsdfsadfasd = ex.Message;
                return null;
            }
        }


        #endregion


    }
}
