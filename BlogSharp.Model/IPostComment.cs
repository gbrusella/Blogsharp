﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSharp.Model
{
	public interface IPostComment:IIdentifiable<int>
	{
		IPost Post { get; set; }
		string Comment { get; set; }
	}
}
