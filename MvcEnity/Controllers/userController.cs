using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEnity.Models;

namespace MvcEnity.Controllers
{
    public class userController : Controller
    {
        //
        // GET: /user/
        usercascadEntities4 ts = new usercascadEntities4();
         
        public ActionResult Index()
        {
           
            //userform model = new userform();
            var db = ts.empstates.ToList();
            List<userform> li = new List<userform>();
           
            foreach (var i in db)
            {
                userform f = new userform();
                f.id = i.id;
                f.firstname = i.firstname;
                f.lastname = i.lastname;
                f.email = i.email;
                f.address = i.address;
                f.countryid = ts.countries.Where(x => x.countryid == i.countryid).SingleOrDefault().countryname;
                f.stateid = ts.states1.Where(x => x.stateid == i.stateid).SingleOrDefault().statename;
                f.cityid = ts.city2.Where(x=>x.cityid==i.cityid).SingleOrDefault().cityname;
                f.number = i.number;
                li.Add(f);
            }
            return View(li);
           
        }

      
        //
        // GET: /user/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /user/Create

        public ActionResult Create()
        {

            List<ddl> ddl = new List<Models.ddl>()
            {
                new ddl
                {
                    id=0,
                    name="Please select"
                }
            };
            
            userform f = new userform();
            var countrylist = ts.countries.ToList();
            f.countries = new SelectList(countrylist,"countryid","countryname");

            f.states = new SelectList(ddl, "id", "name");
            var citylist = ts.city2.ToList();
            var statelist = ts.states1.ToList();
            f.cities = new SelectList(ddl, "id", "name");
            
            return View(f);
       
        } 

        //
        // POST: /user/Create

        [HttpPost]
        public ActionResult Create(userform uf)
        {
            userform p = new userform();
            try
            {
                List<ddl> ddl = new List<Models.ddl>()
                {
                    new ddl
                    {
                        id=0,
                        name="Please select"
                    }
                };
                var countrylist = ts.countries.ToList();
                p.countries = new SelectList(countrylist, "countryid", "countryname");

                var statelist = ts.states1.ToList();
                p.states = new SelectList(ddl, "id", "name");

                var citylist = ts.city2.ToList();
                p.cities = new SelectList(ddl, "id", "name");
                // TODO: Add insert logic here

                var task = ts.empstates.Where(x => x.email == uf.email).SingleOrDefault();
                if (task == null)
                {
                    empstate f = new empstate();
                    f.firstname = uf.firstname;
                    f.lastname = uf.lastname;
                    f.email = uf.email;
                    f.address = uf.address;
                    f.countryid = Convert.ToInt32(uf.countryid);
                    f.stateid = Convert.ToInt32(uf.stateid);
                    f.cityid =Convert.ToInt32(uf.cityid);
                    f.number = uf.number;
                    ts.empstates.AddObject(f);
                    ts.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already email Exist";
                    return Json(ViewBag.Message);
                }
            }
            catch
            {
                return View(p);
            }
           
        }
        
        //
        // GET: /user/Edit/5
 
        public ActionResult Edit(int id)
        {
           
            userform f = new userform();
            // 
             var countrylist = ts.countries.ToList();
            var t = ts.empstates.Where(x => x.id == id).SingleOrDefault();
            var statelist = ts.states1.Where(x => x.countryid==t.countryid).ToList();
            var citylist = ts.city2.Where(x => x.stateid == t.stateid).ToList();
            f.countries = new SelectList(countrylist, "countryid", "countryname");
            f.states = new SelectList(statelist, "stateid", "statename");
            f.cities = new SelectList(citylist, "cityid", "cityname");

            f.id = t.id;
            f.firstname = t.firstname;
            f.lastname = t.lastname;
            f.email = t.email;
            f.address = t.address;
            f.countryid = t.countryid.ToString();
            f.stateid = t.stateid.ToString();
            f.cityid = t.cityid.ToString();
            f.number = t.number;
            return View(f);
            
        }

        //
        // POST: /user/Edit/5

        [HttpPost]
        public ActionResult Edit(userform uf)
        {
            var countrylist = ts.countries.ToList();
            uf.countries = new SelectList(countrylist, "countryid", "countryname");
            var citylist = ts.city2.ToList();
            uf.cities = new SelectList(citylist, "cityid", "cityname");
            var statelist = ts.states1.ToList();
            uf.states = new SelectList(statelist, "stateid", "statename");
            try
            {

                // TODO: Add update logic here
                var t = ts.empstates.Where(x => x.id==uf.id).SingleOrDefault();
                t.id = uf.id;
                t.firstname = uf.firstname;
                t.lastname = uf.lastname;
                t.email = uf.email;
                t.address = uf.address;
                t.countryid = Convert.ToInt32(uf.countryid);
                t.stateid = Convert.ToInt32(uf.stateid);
                t.cityid = Convert.ToInt32(uf.cityid);
                t.number = uf.number;
                ts.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(uf);
            }
        }

        //
        // GET: /user/Delete/5
 
        public ActionResult Delete(int id)
        {
            var t = ts.empstates.Where(x => x.id == id).SingleOrDefault();
            ts.DeleteObject(t);
            ts.SaveChanges();
            return RedirectToAction("Index");
          
        }

        //
        // POST: /user/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetcityByStateId(int stateid)
        {
            using (usercascadEntities4 db = new usercascadEntities4())
            {
                List<city2> lstStates = new List<city2>();
                lstStates = db.city2.Where(x => x.stateid == stateid).ToList();
                userform f = new userform();
                f.cities = new SelectList(lstStates, "cityid", "cityname");
                return Json(f.cities, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetstateBycountryId(int countryid) 
        {
            using (usercascadEntities4 db = new usercascadEntities4())
            {
                List<states1> lstStates = new List<states1>();
                lstStates = db.states1.Where(x => x.countryid == countryid).ToList();
                userform f = new userform();
                f.cities = new SelectList(lstStates, "stateid", "statename");
                return Json(f.cities, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
