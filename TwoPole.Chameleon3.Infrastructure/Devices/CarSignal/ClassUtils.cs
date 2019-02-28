using Android.Bluetooth;
using Android.Util;
using Java.Lang;
using Java.Lang.Reflect;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    public class ClsUtils
    {
        static public bool createBond(Class btClass, BluetoothDevice btDevice)
        {
            Method createBondMethod = btClass.GetMethod("createBond");
            Boolean returnValue = (Boolean)createBondMethod.Invoke(btDevice);
            return returnValue.BooleanValue();
        }


        static public bool removeBond(Class btClass, BluetoothDevice btDevice)
        {
            Method removeBondMethod = btClass.GetMethod("removeBond");
            Boolean returnValue = (Boolean)removeBondMethod.Invoke(btDevice);
            return returnValue.BooleanValue();
        }

        static public bool setPin(Class btClass, BluetoothDevice btDevice, string str)
        {
            try
            {
                // Method removeBondMethod = btClass.GetDeclaredMethod("setPin", new Class[] { byte[].class}); 
                //Method removeBondMethod = btClass.GetDeclaredMethod("SetPin", new Class[] {Class.FromType(typeof(Java.Lang.Byte[]))});
                //Boolean returnValue = (Boolean)removeBondMethod.Invoke(btDevice, new Object[] { System.Text.Encoding.ASCII.GetBytes(str) });
                btDevice.SetPin(System.Text.Encoding.ASCII.GetBytes(str));
            }
     
            catch (Exception e)
            {
                // TODO Auto-generated catch block    
                e.PrintStackTrace();
            }
            return true;
        }

        // 取消用户输入    
        static public bool cancelPairingUserInput(Class btClass, BluetoothDevice device)
        {
            Method createBondMethod = btClass.GetMethod("cancelPairingUserInput");
            // cancelBondProcess()    
            Boolean returnValue = (Boolean)createBondMethod.Invoke(device);
            return returnValue.BooleanValue();
        }

        // 取消配对    
        static public bool cancelBondProcess(Class btClass,
                BluetoothDevice device)
        {
            Method createBondMethod = btClass.GetMethod("cancelBondProcess");
            Boolean returnValue = (Boolean)createBondMethod.Invoke(device);
            return returnValue.BooleanValue();
        }


        static public void printAllInform(Class clsShow)
        {
            try
            {
                // 取得所有方法    
                Method[] hideMethod = clsShow.GetMethods();
                int i = 0;
                for (; i < hideMethod.Length; i++)
                {
                    Log.Error("method name", hideMethod[i].Name + ";and the i is:"
                            + i);
                }
                // 取得所有常量    
                Field[] allFields = clsShow.GetFields();
                for (i = 0; i < allFields.Length; i++)
                {
                    Log.Error("Field name", allFields[i].Name);
                }
            }
            catch (SecurityException e)
            {
                // throw new RuntimeException(e.getMessage());    
                e.PrintStackTrace();
            }
            catch (IllegalArgumentException e)
            {
                // throw new RuntimeException(e.getMessage());    
                e.PrintStackTrace();
            }
            catch (Exception e)
            {
                // TODO Auto-generated catch block    
                e.PrintStackTrace();
            }
        }
    }
}