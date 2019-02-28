<Query Kind="Program" />

void Main()
{
	 var lines = File.ReadAllLines(@"D:\Projects\chameleon3\AndroidApp\AndroidApp\TwoPole.Chameleon3\TwoPole.Chameleon3.Aisound\Transforms\错误清单.txt");
	 var query = from line in lines.Select(x => x.Trim())
	 			 where line.Length > 0 && line.StartsWith("类型")
				 select line;	 
	 var attrs = new List<string>(); 
	 foreach(var line in query)
	 {
	 	var reg = Regex.Match(line, "^类型“([a-z]+)”", RegexOptions.IgnoreCase);
		if(reg.Success)
		{
			var items = line.Split("\t".ToCharArray());
			if(items.Length != 2)
			{
				("格式忽略：" + line).Dump();				
			}
			else 
			{
				// event args for com.iflytek.cloud.VerifierListener.onEvent
				// Metadata.xml XPath interface reference: path="/api/package[@name='com.iflytek.cloud']/interface[@name='IdentityListener']"		
				// 类型“EventEventArgs”已经包含“p1”的定义	Com.Iflytek.Cloud.IVerifierListener.cs
				//com.someapp.android.mpa.guidance.NavigationManager.on2DSignNextManuever(NextManueverListener listener);
				//<attr path="/api/package[@name='com.someapp.android.mpa.guidance']/interface[@name='NavigationManager.Listener']/method[@name='on2DSignNextManeuver']" name="argsType">NavigationManager.TwoDSignNextManueverEventArgs</attr>
				var names = Path.GetFileName(items[1]).Split('.');
				var packageName = string.Join(".", names.Take(names.Length - 2)).ToLower();
				var interfaceName = (names[names.Length - 2]);
				interfaceName = interfaceName.Substring(1);
				var methodName = reg.Groups[1].Value.Replace("EventArgs", "");
				var newInterfaceName = interfaceName.Replace("Listener", "");
				var attr = string.Format("<attr path=\"/api/package[@name='{0}']/interface[@name='{1}']/method[@name='on{2}']\" name=\"argsType\">{3}{2}EventArgs</attr>",
					packageName,
					interfaceName,
					methodName,
					newInterfaceName);				
				//line.Dump();
				attr.Dump();
				attrs.Add(attr);
			}
		}
		else 
		{
			("不匹配忽略：" + line).Dump();
		}
	 }
	 File.WriteAllLines(@"D:\Projects\chameleon3\AndroidApp\AndroidApp\TwoPole.Chameleon3\TwoPole.Chameleon3.Aisound\Transforms\Metadata2.xml", attrs.Distinct());
}

// Define other methods and classes here
