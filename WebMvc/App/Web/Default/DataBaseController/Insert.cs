﻿using System;
using System.Collections.Generic;
using System.Web;

namespace WebMvc.App.Web.Default.DataBaseController
{
    public class InsertAction  : Controller
	{
		public InsertAction(System.IO.TextWriter tw):base(tw){}
		public InsertAction(string fileName) : base(fileName) {}
        #region 查询
        //查询首行首列
        
        //查询所有行
        
        //查询随机行
        
        //查询前多少行
        
        //查询一行
        

        #endregion
        #region 增删改
        
        public void Insert()
        { 
        
        }
        
        
        #endregion
        #region 父子表,多表关系
        
        #endregion
    }
}