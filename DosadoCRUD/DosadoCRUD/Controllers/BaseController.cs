using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DosadoCRUD.Repository;

namespace DosadoCRUD.Controllers
{
    public class BaseController : Controller
    {
        public DBSYS32Entities _db;
        public BaseRepository<User> _userRepo;
        public BaseController()
        {
            _db = new DBSYS32Entities();
            _userRepo = new BaseRepository<User>();
        }
    }
}