using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nop.Core.Domain.Omni_Backoffice;
using Nop.Core;

namespace Nop.Services.Omni_Backoffice
{
    public partial interface IClassService 
    {
        Teacher GetTeacherById(int teacherID);

        void InsertTeacher(Teacher teacher);

        void DeleteTeacher(Teacher teacher);

        void UpdateTeacher(Teacher teacher);

        IPagedList<Teacher> GetAllTeachers(string Branch, string FirstName, string LastName, string Subject, int pageIndex = 0, int pageSize = 2147483647);


        TeacherCareer GetTeacherCareerById(int teachercareerID);

        void InsertTeacherCareer(TeacherCareer teachercareer);

        void DeleteTeacherCareer(TeacherCareer teachercareer);

        void UpdateTeacherCareer(TeacherCareer teachercareer);

        IPagedList<TeacherCareer> GetAllTeacherCareers(int teacherid, int pageIndex = 0, int pageSize = 2147483647);



        Evaluation GetEvaluationById(int evaluationID);

        void InsertEvaluation(Evaluation evaluation);

        void DeleteEvaluation(Evaluation evaluation);

        void UpdateEvaluation(Evaluation evaluation);

        IPagedList<Evaluation> GetAllEvaluations(int teacherid, int pageIndex = 0, int pageSize = 2147483647);



        TeacherSubject GetTeacherSubjectById(int teachersubjectID);

        void InsertTeacherSubject(TeacherSubject teachersubject);

        void DeleteTeacherSubject(TeacherSubject teachersubject);

        void UpdateTeacherSubject(TeacherSubject teachersubject);

        IPagedList<TeacherSubject> GetAllTeacherSubjects(int teacherid, int pageIndex = 0, int pageSize = 2147483647);

        IPagedList<TeacherSubject> GetAllClassTeacherSubjects( int pageIndex = 0, int pageSize = 2147483647);



        TeacherBranch GetTeacherBranchById(int teacherbranchID);

        void InsertTeacherBranch(TeacherBranch teacherbranch);

        void DeleteTeacherBranch(TeacherBranch teacherbranch);

        void UpdateTeacherBranch(TeacherBranch teacherbranch);

        IPagedList<TeacherBranch> GetAllTeacherBranchs(int teacherid, int pageIndex = 0, int pageSize = 2147483647);


        ClassRoom GetClassRoomById(int classroomID);

        void InsertClassRoom(ClassRoom classroom);

        void DeleteClassRoom(ClassRoom classroom);

        void UpdateClassRoom(ClassRoom classroom);

        IPagedList<ClassRoom> GetAllClassRooms(string branch, string title, int pageIndex = 0, int pageSize = 2147483647);



        ClassInfo GetClassInfoById(int classinfoID);

        void InsertClassInfo(ClassInfo classinfo);

        void DeleteClassInfo(ClassInfo classinfo);

        void UpdateClassInfo(ClassInfo classinfo);

        IPagedList<ClassInfo> GetAllClassInfos(string branch, string year, string term, int pageIndex = 0, int pageSize = 2147483647);


        ClassTeacher GetClassTeacherById(int classId);

        void InsertClassTeacher(ClassTeacher classteacher);

        void DeleteClassTeacher(ClassTeacher classteacher);

        void UpdateClassTeacher(ClassTeacher classteacher);

        IPagedList<ClassTeacher> GetAllClassTeachers(int classid, int pageIndex = 0, int pageSize = 2147483647);


        ClassEnrol GetClassEnrolById(int classId);

        void InsertClassEnrol(ClassEnrol classenrol);

        void DeleteClassEnrol(ClassEnrol classenrol);

        void UpdateClassEnrol(ClassEnrol classenrol);

        IPagedList<ClassEnrol> GetAllClassEnrols(int classid, int pageIndex = 0, int pageSize = 2147483647);



        ClassSchedule GetClassScheduleById(int classId);

        void InsertClassSchedule(ClassSchedule classschedule);

        void DeleteClassSchedule(ClassSchedule classschedule);

        void UpdateClassSchedule(ClassSchedule classschedule);

        IPagedList<ClassSchedule> GetAllClassSchedules(int classid, int pageIndex = 0, int pageSize = 2147483647);


        ClassScheduleTeacher GetClassScheduleTeacherById(int classId);

        void InsertClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher);

        void DeleteClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher);

        void UpdateClassScheduleTeacher(ClassScheduleTeacher classscheduleteacher);

        IPagedList<ClassScheduleTeacher> GetAllClassScheduleTeachers(int classid, int pageIndex = 0, int pageSize = 2147483647);



        ClassScheduleRollcall GetClassScheduleRollcallById(int classId);

        void InsertClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall);

        void DeleteClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall);

        void UpdateClassScheduleRollcall(ClassScheduleRollcall classschedulerollcall);

        IPagedList<ClassScheduleRollcall> GetAllClassScheduleRollcalls(int classid, int classdid, int pageIndex = 0, int pageSize = 2147483647);




        ClassEnrol_Pay GetClassEnrol_PayById(int classId);

        void InsertClassEnrol_Pay(ClassEnrol_Pay classenrol);

        void DeleteClassEnrol_Pay(ClassEnrol_Pay classenrol);

        void UpdateClassEnrol_Pay(ClassEnrol_Pay classenrol);

        IPagedList<ClassEnrol_Pay> GetAllClassEnrol_Pays(int classenrolid, int pageIndex = 0, int pageSize = 2147483647);

        
        IPagedList<Member> GetAllMembers(string branch, string grade, string firstname, string lastname, string stud_id, string id_number, int pageIndex = 0, int pageSize = 2147483647);



        Parent GetParentById(int parentID);

        void InsertParent(Parent parent);

        void DeleteParent(Parent parent);

        void UpdateParent(Parent parent);

        IPagedList<Parent> GetAllParents(string branch, string firstname, string lastname, int pageIndex = 0, int pageSize = 2147483647);


        SiblingManage GetSiblingManageById(int siblingmanageID);

        void InsertSiblingManage(SiblingManage siblingmanage);

        void DeleteSiblingManage(SiblingManage siblingmanage);

        void UpdateSiblingManage(SiblingManage siblingmanage);

        IPagedList<SiblingManage> GetAllSiblingManages( int parent_id, int pageIndex = 0, int pageSize = 2147483647);


        AdditionInfo GetAdditionInfoById(int additioninfoid);

        void InsertAdditionInfo(AdditionInfo additioninfo);

        void DeleteAdditionInfo(AdditionInfo additioninfo);

        void UpdateAdditionInfo(AdditionInfo additioninfo);

        IPagedList<AdditionInfo> GetAllAdditionInfos(string studid, int pageIndex = 0, int pageSize = 2147483647);


        StudentBranch GetStudentBranchById(int studentbranchId);

        void InsertStudentBranch(StudentBranch studentbranch);

        void DeleteStudentBranch(StudentBranch studentbranch);

        void UpdateStudentBranch(StudentBranch studentbranch);

        IPagedList<StudentBranch> GetAllStudentBranchs(string studid, string branch, int pageIndex = 0, int pageSize = 2147483647);


    }
}
