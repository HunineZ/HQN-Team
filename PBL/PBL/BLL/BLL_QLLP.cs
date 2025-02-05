﻿using System.Collections.Generic;
using System.Linq;
using PBL.DAL;

namespace PBL.BLL
{
    class BLL_QLLP
    {
        private static BLL_QLLP _Instance;
        public static BLL_QLLP Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new BLL_QLLP();
                }
                return _Instance;
            }
            private set { }
        }
        private BLL_QLLP() { }
        public List <LOAIPHONG> GetListLoaiPhong(string s)
        {
            if (s != null)
            {
                return new QLKS().LOAIPHONGs.Where(lp => lp.TenLoaiPhong.ToLower().Contains(s)).ToList();
            }
            return new QLKS().LOAIPHONGs.ToList();
        }
        public LOAIPHONG FindLoaiPhong(string tenloaiphong)
        {
            QLKS db = new QLKS();
            foreach(LOAIPHONG i in db.LOAIPHONGs)
            {
                if (i.TenLoaiPhong.Equals(tenloaiphong))
                {
                    return i;
                }
            }
            return null;
        }

        public LOAIPHONG FindLoaiPhongByID(string id)
        {
            return new QLKS().LOAIPHONGs.Find(id);
        }

        public bool DeleteLoaiPhong(List<string> l)
        {
            try
            {
                QLKS db = new QLKS();
                foreach(string s in l)
                {
                    db.LOAIPHONGs.Remove(db.LOAIPHONGs.Find(s));
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void AddLoaiPhong(LOAIPHONG lp)
        {
            QLKS db = new QLKS();
            db.LOAIPHONGs.Add(lp);
            db.SaveChanges();
        }
        public void UpdateLoaiPhong(LOAIPHONG lp)
        {
            QLKS db = new QLKS();
            var lp1 = db.LOAIPHONGs.Find(lp.LoaiPhongID);
            lp1.TenLoaiPhong = lp.TenLoaiPhong;
            lp1.Gia = lp.Gia;
            lp1.SoNguoi = lp.SoNguoi;
            db.SaveChanges();
        }
        public List<LOAIPHONG> Sort(string s, List<string> l)
        {
            List<LOAIPHONG> data = new List<LOAIPHONG>();
            foreach(string i in l)
            {
                data.Add(FindLoaiPhong(i));
            }
            switch(s)
            {
                case "Tên loại phòng":
                    data = data.OrderByDescending(p => p.TenLoaiPhong).ToList();
                    break;
                case "Số người":
                    data = data.OrderByDescending(p => p.SoNguoi).ToList();
                    break;
                case "Giá":
                    data = data.OrderByDescending(p => p.Gia).ToList();
                    break;
            }
            return data;
        }
    }
}
