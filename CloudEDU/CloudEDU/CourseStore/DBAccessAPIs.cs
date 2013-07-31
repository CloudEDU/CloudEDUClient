using CloudEDU.Common;
using CloudEDU.Service;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    class DBAccessAPIs
    {
        private CloudEDUEntities ctx = null;


        public DBAccessAPIs()
        {


            ctx = new CloudEDUEntities(new Uri(Constants.DataServiceURI));
        }


        public DataServiceQuery<CUSTOMER> customerDsq = null;
        public DataServiceQuery<LESSON> lessonDsq = null;
        public DataServiceQuery<RESOURCE> resourceDsq = null;
        public DataServiceQuery<NOTE> noteDsq = null;
        public DataServiceQuery<RES_TYPE> resTypeDsq = null;
        public DataServiceQuery<COURSE> courseDsq = null;
        public delegate string deleMethod(int a, string b);

        public static string test(deleMethod m)
        {

            
            return m(1, "aaa");
            
        }



        public delegate void onQueryComplete(IAsyncResult result);
        private onQueryComplete onUQC;

        

        public void getUserById(int id, onQueryComplete onComplete)
        {
            customerDsq = (DataServiceQuery<CUSTOMER>)(from cus in ctx.CUSTOMER where cus.ID == Constants.User.ID select cus);
            this.onUQC = onComplete;
            customerDsq.BeginExecute(onQueryComplete2, null);
        }


        private void onQueryComplete2(IAsyncResult result)
        {
            this.onUQC(result);
        }



        public void GetLessonById(int id, onQueryComplete onComplete)
        {
            lessonDsq = (DataServiceQuery<LESSON>)(from les in ctx.LESSON where les.ID == id select les);
            this.onUQC = onComplete;
            lessonDsq.BeginExecute(onQueryComplete2, null);
        }

        public void GetLessonsByCourseId(int id, onQueryComplete onComplete)
        {
            lessonDsq = (DataServiceQuery<LESSON>)(from les in ctx.LESSON where les.COURSE_ID == id select les);
            this.onUQC = onComplete;
            lessonDsq.BeginExecute(onQueryComplete2, null);
        }


        public void GetResourcesByLessonId(int id, onQueryComplete onComplete)
        {
            resourceDsq = (DataServiceQuery<RESOURCE>)(ctx.RESOURCE.Where(r => r.LESSON_ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onQueryComplete2, null);
        }

        public void GetNoteByCustomerID(int id, onQueryComplete onComplete)
        {
            noteDsq = (DataServiceQuery<NOTE>)(ctx.NOTE.Where(n => n.CUSTOMER_ID == id));
            this.onUQC = onComplete;
            noteDsq.BeginExecute(onQueryComplete2, null);
        }

        public void GetTypeByResourceID(int id, onQueryComplete onComplete)
        {
            resourceDsq = (DataServiceQuery<RESOURCE>)(ctx.RESOURCE.Where(r => r.ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onResComplete, null);
        }


        public void GetNoteByLessonId(int id, onQueryComplete onComplete)
        {
            noteDsq = (DataServiceQuery<NOTE>)(from note in ctx.NOTE where note.LESSON_ID == id select note);
            this.onUQC = onComplete;
            noteDsq.BeginExecute(onQueryComplete2, null);
        }

        public void GetResourceByID(int id, onQueryComplete onComplete)
        {
            resourceDsq = (DataServiceQuery<RESOURCE>)(ctx.RESOURCE.Where(r => r.ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onQueryComplete2, null);
        }

        private void onResComplete(IAsyncResult res)
        {
            IEnumerable<RESOURCE> resources = resourceDsq.EndExecute(res);
            RESOURCE resource = resources.FirstOrDefault();
            resTypeDsq = (DataServiceQuery<RES_TYPE>)(ctx.RES_TYPE.Where(r => r.ID == resource.TYPE));
            resTypeDsq.BeginExecute(onQueryComplete2, null);
        }

        public void InsertNote(NOTE note, onQueryComplete onComplete)
        {
            ctx.AddToNOTE(note);
            this.onUQC = onComplete;
            ctx.BeginSaveChanges(onQueryComplete2, null);                        
        }

        public void EditNote(NOTE note, onQueryComplete onComplete)
        {
            ctx.UpdateObject(note);
            this.onUQC = onComplete;
            ctx.BeginSaveChanges(onQueryComplete2, null);
        }

        public void EditLesson(LESSON lesson, onQueryComplete onComplete)
        {
            ctx.UpdateObject(lesson);
            this.onUQC = onComplete;
            ctx.BeginSaveChanges(onQueryComplete2, null);
        }

        public void EditCourse(COURSE course, onQueryComplete onComplete)
        {
            ctx.UpdateObject(course);
            this.onUQC = onComplete;
            ctx.BeginSaveChanges(onQueryComplete2, null);
        }

        public void DeleteNote(NOTE note, onQueryComplete onComplete)
        {
            ctx.DeleteObject(note);
            this.onUQC = onComplete;
            ctx.BeginSaveChanges(onQueryComplete2, null);
        }

        public void GetNoteByID(int id, onQueryComplete onComplete)
        {
            noteDsq = (DataServiceQuery<NOTE>)(ctx.NOTE.Where(r => r.ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onResComplete, null);
        }

        public void GetLessonByID(int id, onQueryComplete onComplete)
        {
            lessonDsq = (DataServiceQuery<LESSON>)(ctx.LESSON.Where(r => r.ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onResComplete, null);
        }

        public void GetCourseByID(int id, onQueryComplete onComplete)
        {
            courseDsq = (DataServiceQuery<COURSE>)(ctx.COURSE.Where(r => r.ID == id));
            this.onUQC = onComplete;
            resourceDsq.BeginExecute(onResComplete, null);
        }
    }
}
