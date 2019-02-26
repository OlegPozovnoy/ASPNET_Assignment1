using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment1.Models;

namespace Assignment1.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {
        private DbModel db = new DbModel();

        // GET: Tests
        public ActionResult Index()
        {
            return View(db.Tests.ToList());
        }

        // GET: Tests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // GET: Tests/Create
        [Authorize (Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "Id,author,name,description")] Test test)
        {
            if (ModelState.IsValid)
            {
                db.Tests.Add(test);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(test);
        }

        // GET: Tests/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }


        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "Id,author,name,description")] Test test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test);
        }
        [Authorize]
        public ActionResult Take(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(int testId) {

            string testResultKey ="";

            foreach (var key in HttpContext.Request.Form.AllKeys)
            {

                if (key.ToString().Equals("__RequestVerificationToken"))
                {
                    testResultKey = HttpContext.Request.Form[key];
                    RecordTestAttemptToDB(testResultKey, HttpContext.User.Identity.Name, testId);
                }
                else if (key.Equals("testId") == false)
                {

                    RecorsAnswerToDB(testResultKey, Convert.ToInt32(key), Convert.ToInt32(HttpContext.Request.Form[key]));
                }
            }


             Tuple<int,int> result =  CalculateResults(testResultKey);

            ViewBag.Score = result.Item1;
            ViewBag.MaxScore = result.Item2;

            //db.Database.SqlQuery

          //  ViewBag.data = result;
            return View();
        }


        private Tuple<int, int> CalculateResults(string attemptKey) {

            string command = "update dbo.TestResultsAnswers " +
                         "set score = A.Score " +
                         "from dbo.Answers A " +
                         "where A.Id = dbo.TestResultsAnswers.AnswerId " +
                         "and TestResultsId = '" + attemptKey + "'";

            db.Database.ExecuteSqlCommand(command);

            command ="update dbo.TestResultsAnswers " +
                            "set MaxScore = T.MaxScore " +
                            "from " +
                            "(select QuestionId, max(score) as MaxScore from dbo.Answers " +
                            "group by QuestionId) T " +
                            "where dbo.TestResultsAnswers.QuestionId = T.QuestionId " +
                            "and TestResultsId = '" + attemptKey + "'";

            db.Database.ExecuteSqlCommand(command);

            command = "update dbo.TestResults " +
                        "set Scored = T.Score, " +
                        "MaxScore = T.MaxScore " +
                        "from " +
                        "(select sum(Score) as Score, sum(MaxScore) as MaxScore from TestResultsAnswers where TestResultsId = '" + attemptKey + "') T " +
                        "where TestResultsId = '" + attemptKey + "' ";

            db.Database.ExecuteSqlCommand(command);

            command =   " select top 1 Scored, MaxScore from TestResults " +
                        " where TestResultsId = 'test'";

            // IEnumerable<Tuple<int,int>> data = db.Database.SqlQuery<Tuple<int, int>>(command);

            command = " select top 1 Scored from TestResults " +
                        " where TestResultsId = '" + attemptKey + "'";

            int scored = db.Database.SqlQuery<int>(command).FirstOrDefault();
            //var x = data.ToList();

            command = " select top 1 MaxScore from TestResults " +
            " where TestResultsId = '" + attemptKey + "'";

            int maxscore = db.Database.SqlQuery<int>(command).FirstOrDefault(); ;
            //var y = data.ToArray();

            return new Tuple<int,int>(scored, maxscore);
        }


        private int RecorsAnswerToDB(string attemptKey, int questionKey, int answerKey) {
            string command = "insert into [dbo].[TestResultsAnswers] ([TestResultsId], [QuestionId], [AnswerId], [Score], [MaxScore])" +
	            "values('" + attemptKey + "'," + questionKey + "," + answerKey + ", null,null)"  ;

            db.Database.ExecuteSqlCommand(command);

            return 1;
        }

        private int RecordTestAttemptToDB(string attemptKey, string User, int testId) {

            string command = "insert into dbo.TestResults(TestResultsId, StartTime, EndTime, [User], Scored, MaxScore, TestId) " +
                "values('" + attemptKey + "', null, getdate(), '" + User + "', null, null, " + testId + ")";

            db.Database.ExecuteSqlCommand(command);

            return 1;
        }

        // GET: Tests/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
