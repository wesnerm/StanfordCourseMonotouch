#region Using

/////////////////////////////////////////////////////////////////////////////
// This source code may not be reviewed, copied, or redistributed without
// the expressed permission of Wesner Moise.
// 
// Copyright (C) 2002-2010, Wesner Moise.
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

#endregion

namespace WM
{
#pragma warning disable 414
	// ReSharper disable MethodOverloadWithOptionalParameter

	[DebuggerNonUserCode]
	[DebuggerStepThrough]
	public static class Utility
	{
		#region General
		[DebuggerStepThrough]
		public static string DotNetBarLicenseKey()
		{
			return "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
		}

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

		/// <summary>
		/// Swaps two variables
		/// </summary>
		/// <param name="var1">first variable</param>
		/// <param name="var2">second variable</param>
		public static void Swap<T>(ref T var1, ref T var2)
		{
			T tmp = var1;
			var1 = var2;
			var2 = tmp;
		}

		public static void Normalize<T>(ref T var1, ref T var2)
			where T : IComparable<T>
		{
			if (var1 != null && var1.CompareTo(var2) > 0)
				Swap(ref var1, ref var2);
		}

		private static Func<object, object> _cloner;

		public static T Clone<T>(T o)
		{
			if (_cloner == null)
			{
				MethodInfo info = typeof (object).GetMethod("MemberwiseClone",
				                                            BindingFlags.Instance | BindingFlags.NonPublic);
				_cloner = (Func<object, object>)
				         Delegate.CreateDelegate(typeof (Func<object, object>), info);
			}
// ReSharper disable PossibleNullReferenceException
			return (T) _cloner(o);
// ReSharper restore PossibleNullReferenceException
		}



		public static T New<T>()
		{
			return (T) FormatterServices.GetUninitializedObject(typeof (T));
		}

		#endregion

		#region Math

		public static int Hex(string text)
		{
			return int.Parse(text, NumberStyles.HexNumber);
		}

		public static T Max<T>(params T[] numbers)
			where T : IComparable<T>
		{
			if (numbers.Length == 0)
				return default(T);

			T result = numbers[0];
			for (int i = 1; i < numbers.Length; i++)
				if (result.CompareTo(numbers[i]) < 0)
					result = numbers[i];

			return result;
		}

		public static T Min<T>(params T[] numbers)
			where T : IComparable<T>
		{
			if (numbers.Length == 0)
				return default(T);

			T result = numbers[0];
			for (int i = 1; i < numbers.Length; i++)
				if (result.CompareTo(numbers[i]) > 0)
					result = numbers[i];

			return result;
		}

		public static float Sum(params float[] numbers)
		{
			double sum = 0;
			for (int i = 0; i < numbers.Length; i++)
				sum += numbers[i];
			return (float) sum;
		}

		public static double Average(params double[] numbers)
		{
			return Sum(numbers)/numbers.Length;
		}

		public static double Sum(params double[] numbers)
		{
			double sum = 0;
			foreach (double t in numbers)
			    sum += t;
		    return sum;
		}

		public static double SumSquared(params double[] numbers)
		{
			double sum = 0;
			foreach (double n in numbers)
			    sum += n*n;
			return sum;
		}

		public static int Sum(params int[] numbers)
		{
			int sum = 0;
			foreach (int t in numbers)
			    sum += t;
		    return sum;
		}

		[DebuggerStepThrough]
		public static int Sqr(int number)
		{
			return number*number;
		}

		[DebuggerStepThrough]
		public static double Sqr(double number)
		{
			return number*number;
		}

		#endregion

		#region Logical

		[DebuggerStepThrough]
		public static int RemoveLastBit(int value)
		{
			return value & value - 1;
		}




		#endregion

		#region Random Numbers

		private static readonly Random _random = new Random();

		[DebuggerStepThrough]
		public static double RandomDouble()
		{
			return _random.NextDouble();
		}

		[DebuggerStepThrough]
		public static int RandomInteger()
		{
			return _random.Next();
		}

		[DebuggerStepThrough]
		public static int RandomInteger(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}

		#endregion

		#region Hash Code

		public const double GoldenRatio = 1.618033988749895;

		[DebuggerStepThrough]
		public static int CreateHashCodeRange(int value, int range = int.MaxValue)
		{
			unchecked
			{
				const long goldenRatioBits = 2654435769;
				long hash = (uint) (value*goldenRatioBits);
				return (int) ((hash*(uint) range) >> 32);
			}
		}

		[DebuggerStepThrough]
		public static int CreateHashCode(int first)
		{
			return CreateHashCodeRange(first);
		}

		[DebuggerStepThrough]
		public static int CreateHashCode(int first, int second)
		{
			return CreateHashCode(CreateHashCode(first) ^ second);
		}

		[DebuggerStepThrough]
		public static int CreateHashCode(int first, int second, int third)
		{
			return CreateHashCode(first, CreateHashCode(second, third));
		}

		public static int GetHashCode(object obj)
		{
			return obj == null ? 0x1234567 : obj.GetHashCode();
		}

		[DebuggerStepThrough]
		public static int CreateHashCode<T>(IList<T> array)
		{
			if (array == null) return 0;
			return CreateHashCode(array, 0, array.Count);
		}

		[DebuggerStepThrough]
		public static int CreateHashCode<T>(IList<T> array, int start, int count)
		{
			if (array == null) return 0;
			int hashcode = CreateHashCode(count);
			for (int i = 0; i < count; i++)
				hashcode = CreateHashCode(hashcode ^ array[start + i].GetHashCode());
			return hashcode;
		}

		[DebuggerStepThrough]
		public static string ComputeMd5Hash(string text)
		{
			if (string.IsNullOrEmpty(text))
				return string.Empty;
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			return ComputeMd5Hash(bytes);
		}

		[DebuggerStepThrough]
		public static string ComputeMd5Hash(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return string.Empty;
			MD5 md = MD5.Create();
			return BitConverter.ToString(md.ComputeHash(bytes));
		}

		private static int _counter;

		public static int UniqueId()
		{
			return Interlocked.Increment(ref _counter);
		}

		#endregion

		#region Logarithm

		public static int EstimateTreeDepth(int leafCount)
		{
			// Red-black tree is at most 2 lg n
			// Treap is on average 1.7 lg n (3.7 average worst case)
			int depth = Log2(leafCount);
			return depth + depth + 1;
		}

		public static int CountBits(int value)
		{
			int count = 0;
			while (value != 0)
			{
				value &= (value - 1);
				count ++;
			}
			return count;
		}

		public static bool HasOneBit(int value)
		{
			return (value & (value - 1)) == 0 && value != 0;
		}

		public static long MaxBits(long bits)
		{
			int log = Log2(bits);
			if (log < 0) return 0;
			return (1 << log) - 1;
		}

		[DebuggerStepThrough]
		public static uint RotateLeft(uint bits, int n = 1)
		{
			var n1 = n & 31;
			return (bits << n1) | (bits >> (32 - n1));
		}

		[DebuggerStepThrough]
		public static uint RotateRight(uint bits, int n = 1)
		{
			var n1 = n & 31;
			return (bits << (32-n1)) | (bits >> n1);
		}

		[DebuggerStepThrough]
		public static int RotateLeft(int bits, int n = 1)
		{
			return unchecked((int) RotateLeft((uint)bits, n));
		}

		[DebuggerStepThrough]
		public static int RotateRight(int bits, int n = 1)
		{
			return unchecked((int)RotateLeft((uint)bits, n));
		}

		[DebuggerStepThrough]
		public static int RotateLeft31(int bits, int n = 1)
		{
			unchecked
			{
				int n1;
				if ((uint)n < 31)
					n1 = n;
				else
				{
					n1 = n %  31;
					if (n1 < 0)
						n1 = 31 + n;
				}

				var b = bits & int.MaxValue;
				return ((b << n1) | b >> (31 - n1)) & int.MaxValue;
			}
		}

		[DebuggerStepThrough]
		public static int RotateRight31(int bits, int n = 1)
		{
			return RotateLeft31(bits, 31-n);
		}


		[DebuggerStepThrough]
		public static ulong RotateLeft(ulong bits, int n = 1)
		{
			var n1 = n & 31;
			return (bits << n1) | (bits >> (32 - n1));
		}

		[DebuggerStepThrough]
		public static ulong RotateRight(ulong bits, int n = 1)
		{
			var n1 = n & 31;
			return (bits << (32 - n1)) | (bits >> n1);
		}

		[DebuggerStepThrough]
		public static long RotateLeft(long bits, int n = 1)
		{
			return unchecked((long)RotateLeft((ulong)bits, n));
		}

		[DebuggerStepThrough]
		public static long RotateRight(long bits, int n = 1)
		{
			return unchecked((long)RotateLeft((ulong)bits, n));
		}


		public static int Log2(long value)
		{
			if (value <= 0)
				return value == 0 ? -1 : 63;

			int log = 0;
			if (value >= 0x100000000L)
			{
				log += 32;
				value >>= 32;
			}
			if (value >= 0x10000)
			{
				log += 16;
				value >>= 16;
			}
			if (value >= 0x100)
			{
				log += 8;
				value >>= 8;
			}
			if (value >= 0x10)
			{
				log += 4;
				value >>= 4;
			}
			if (value >= 0x4)
			{
				log += 2;
				value >>= 2;
			}
			if (value >= 0x2)
			{
				log += 1;
				value >>= 1;
			}
			Assert(value == 1);
			return log;
		}

		#endregion

		#region Min and Max

		public static T Max<T>(this T a, T b) where T : IComparable<T>
		{
			if (a == null) return b;
			if (b == null) return a;
			return a.CompareTo(b) >= 0 ? a : b;
		}

		public static T Min<T>(this T a, T b) where T : IComparable<T>
		{
			if (a == null) return default(T);
			if (b == null) return default(T);
			return a.CompareTo(b) <= 0 ? a : b;
		}

		public static int CompareToEx<T>(this T a, T b) where T : IComparable<T>
		{
			if (a == null) return b == null ? 0 : -1;
			if (b == null) return 1;
			return a.CompareTo(b);
		}

		public static T Limit<T>(this T value, T minValue, T maxValue) where T : IComparable
		{
			if (-1 == value.CompareTo(minValue))
				return minValue;
			if (1 == value.CompareTo(maxValue))
				return maxValue;
			return value;
		}

		public static bool Between<T>(this T value, T minValue, T maxValue) where T : IComparable
		{
			if (-1 == value.CompareTo(minValue))
				return false;
			if (1 == value.CompareTo(maxValue))
				return false;
			return true;
		}

		#endregion

		#region Reflection

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static object InvokeMember(this object obj, string method,
		                                  params object[] args)
		{
			Type type = obj.GetType();
			type.GetMethod(method);
			return type.InvokeMember(method, 0, null, obj, args);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static object GetMember(this object obj, string property)
		{
			return obj.GetType().GetProperty(property).GetValue(obj, null);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetMember(this object obj, string property, object value)
		{
			obj.GetType().GetProperty(property).SetValue(obj, value, null);
		}

		#endregion

		#region Diagnostics

		[DebuggerStepThrough]
		[Conditional("DEBUG")]
		public static void Ignore(object o)
		{
		}

#if DEBUG
		private static readonly Dictionary<string, int> _breakpoints = new Dictionary<string, int>();
		private static string _lastFileLine;
#endif

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Break(int count = 1, int skip = 1)
		{
#if DEBUG
			var frame = new StackFrame(skip, true);
			string text = frame.GetFileName() + ":" + frame.GetFileLineNumber();
			_lastFileLine = text;
			if (!_breakpoints.ContainsKey(text))
				_breakpoints.Add(text, count);

			int passes = _breakpoints[text];
			if (passes <= 0)
				return;

			_breakpoints[text] = passes - 1;
			if (Debugger.IsAttached)
				Debugger.Break();
#endif
		}

		[DebuggerHidden]
		[Conditional("DEBUG")]
		public static void Assert(ref int marker, bool expr)
		{
			if (!expr && marker > 0)
			{
				marker--;
				if (Debugger.IsAttached)
					Debugger.Break();
			}
		}

#if DEBUG
		public static void IncrementBreakpoint(int diff)
		{
			if (_lastFileLine == null)
				return;

			if (diff == 0)
				_breakpoints[_lastFileLine] = 0;
			else
				_breakpoints[_lastFileLine] += diff;
		}
#endif

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Assert(bool expr, string message = null)
		{
			if (!expr) Break(2, 3);
		}

		[DebuggerStepThrough]
		public static bool NotNull<T>(T obj)
		{
			return obj != null;
		}

		[DebuggerStepThrough]
		[Conditional("DEBUG")]
		public static void AssertNotNull<T>(T obj)
		{
			Debug.Assert(obj != null);
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Fail(string message, params object[] args)
		{
			Fail(String.Format(message,args));
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Fail(string message = null)
		{
			Break(2, 3);
		}


		[Conditional("DEBUG")]
		public static void Write(string format, params object[] list)
		{
#if DEBUG
			Debug.Write(String.Format(format, list));
#endif
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string format, params object[] list)
		{
#if DEBUG
			Debug.WriteLine(format, list);
#endif
		}

		public static void FatalError(string message, params object[] args)
		{
			Console.Error.WriteLine(message, args);
			Environment.Exit(1);
		}

		[Conditional("DEBUG")]
		public static void Indent(int space)
		{
			Debug.Write(new String(' ', space));
		}

		[Conditional("DEBUG")]
		public static void WriteProperties(object obj, int indent = 1, 
			bool fields = false, bool baseType = false)
		{
			Type type = obj.GetType();

			Indent(indent);
			WriteLine("{0} ({1})", type.Name, obj.ToString());
			do
			{
				/*
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					try { result = fieldInfo.GetValue(obj); }
					catch(Exception e) { result = e.Message; }				
					Indent(indent);
					WriteLine("  {0} = {1}", fieldInfo.Name, result);
				}
				*/

				foreach (PropertyInfo propertyInfo in type.GetProperties())
				{
					if (!propertyInfo.CanRead || propertyInfo.GetIndexParameters().GetLength(0) > 0)
						continue;
					object result;
					try
					{
						result = propertyInfo.GetValue(obj, null);
					}
					catch (Exception e)
					{
						result = e.Message;
					}

					if (result == null
					    || result is int && (int) result == 0
					    || result is bool && (bool) result == false
					    || result as string == String.Empty)
						continue;

					Indent(indent);
					Type type2;
					if ((type2 = result.GetType()).IsPrimitive && !type2.IsArray)
						WriteLine("{0} := {1}", propertyInfo.Name, result);
					else if (type2.IsArray)
						WriteLine("{0} := {1}[{2}]", propertyInfo.Name, type2.BaseType, ((Array) result).Length);
				}

				type = type.BaseType;
			} while (baseType && type != null && type != typeof (object));
			WriteLine("");
		}

		#endregion

		#region Hexadecimal

		public static int ToHex(char ch)
		{
			if (ch <= '9')
			{
				if (ch < '0') return -1;
				return ch - '0';
			}

			if (ch > 'F')
			{
				if (ch < 'a' || ch > 'f')
					return -1;
				return ch + (10 - 'a');
			}

			if (ch < 'A')
				return -1;
			return ch + (10 - 'A');
		}

		#endregion

		#region NOOP

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void NoOp(object obj = null)
		{
		}

		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void NoOp(params object[] array)
		{
		}

		[DebuggerHidden]
		public static void TryCatch(Action action)
		{
			try
			{
				action();
			}
			catch (Exception)
			{
			}
		}

		[DebuggerHidden]
		public static T TryCatch<T>(Func<T> action)
		{
			try
			{
				return action();
			}
			catch (Exception e)
			{
				NoOp(e);
				return default(T);
			}
		}

		#endregion

		public static double C(int n, int m)
		{
			int n2 = n;

			if (m + m > n)
				m = n - m;

			long product = 1;
			for (int i = 1; i <= m; i++)
				product = (product*n2--)/i;
			return product;
		}

		public static double Fact(int n)
		{
			if (n > 50)
				n = 50;

			double prod = 1;
			for (int i = 2; i <= n; i++)
				prod *= i;
			return prod;
		}

		[Conditional("DEBUG")]
		[DebuggerNonUserCode]
		internal static void DepthCheck<T1, T2>(ref Func<T1, T2> func, int count)
		{
#if DEBUG
			Func<T1, T2> f = func;
			func = delegate(T1 e)
			       	{
			       		Assert(count != 0);
			       		count--;
			       		T2 result = f(e);
			       		count++;
			       		return result;
			       	};
#endif
		}

		//private class DC<T1,T2>
		//{
		//    public int Depth;
		//    public Func<T1, T2> Func;
		//    [DebuggerNonUserCode]
		//    public T2 Main(T1 e)
		//    {
		//        Utility.Assert(Depth != 0);
		//        Depth--;
		//        T2 result = Func(e);
		//        Depth++;
		//        return result;
		//    }
		//}


		#region System

		public static int Shell(string cmd)
		{
			using (var process = new Process
			{
				EnableRaisingEvents = true,
				StartInfo = new ProcessStartInfo
				{
					FileName = "CMD.exe ",
					RedirectStandardError = false,
					RedirectStandardOutput = false,
					RedirectStandardInput = false,
					UseShellExecute = false,
					CreateNoWindow = true,
					Arguments = "/D /c " + cmd,
				}
			})
			{
				process.Start();
				process.WaitForExit(int.MaxValue); //or the wait time you want
				int errorCode = process.ExitCode;

				//Now we need to see if the process was successful
				if (errorCode > 0 & !process.HasExited)
					process.Kill();

				//now clean up after ourselves
				return errorCode;
			}
		}

		public static string QuoteSpaces(string s)
		{
			if (s.Contains(" ") || s.Contains("\""))
				s = "\"" + s.Replace("\"", "") + "\"";
			return s;
		}

		#endregion

        #region Reflection

		public static Exception InnerException(Exception e)
		{
			var inner = e.InnerException;
			if (inner == null)
				return e;
			return InnerException(inner);
		}

		public static int CompareTypes(Type type1, Type type2)
		{
			if (type1 == type2)
				return 0;
			if (type1 == null)
				return -1;
			if (type2 == null)
				return 1;
			return string.CompareOrdinal(type1.Name, type2.Name);
		}



        public static IEnumerable<Type> FindTypes(
            List<Type> list,
            string ns)
        {
            return list.Where(
                t =>
                {
                    var tn = t.Namespace ?? "";
                    return tn.EndsWith(ns)
                        && 
                        (tn.Length==ns.Length 
                        || tn[tn.Length-ns.Length]=='.');
                });
        }

        public static bool SetProperty(object obj, string prop, string value)
        {
            var info = GetPropertyInfo(obj, prop);
            object newValue = value;
            if (info.PropertyType != typeof(string))
            {
                var newType = TypeDescriptor.GetConverter(info.PropertyType);
                if (newType == null)
                    return false;
                newValue = newType.ConvertFrom(value);
            }
            info.SetValue(obj, newValue, null);
            return true;
        }

        public static PropertyInfo GetPropertyInfo(object obj, string prop)
        {
            if (obj == null)
                return null;

            // ReSharper disable AssignNullToNotNullAttribute
            var type = obj.GetType();
            var info = type.GetProperty(prop,
                BindingFlags.Public
                | BindingFlags.Instance
                | BindingFlags.FlattenHierarchy
                | BindingFlags.IgnoreCase,
                null, null, null, null);
            return info;
        }

        #endregion

        #region Bits

        public static int ReverseBits(int value)
        {
            var n = unchecked((uint)value);
            n = n >> 16 | n << 16;
            n = n >> 0x8 & 0x00ff00ff | n << 0x8 & 0xff00ff00;
            n = n >> 0x4 & 0x0f0f0f0f | n << 0x4 & 0xf0f0f0f0;
            n = n >> 0x2 & 0x33333333 | n << 0x2 & 0xcccccccc;
            n = n >> 0x1 & 0x55555555 | n << 0x1 & 0xaaaaaaaa;
            return unchecked((int)n);
        }

        public static long GetUnsignedBits(long data, int pos, int bits)
        {
            int chop = 64 - bits;
            unchecked
            {
                return(long) (((ulong)data << (chop - pos)) >> chop);
            }
        }

	    public static long GetSignedBits(long data, int pos, int bits)
        {
            int chop = 64 - bits;
            return (data << (chop - pos)) >> chop;
        }

        public static int GetUnsignedBits(int data, int pos, int bits)
        {
            int chop = 32 - bits;
            unchecked
            {
                return (int)(((uint)data << (chop - pos)) >> chop);
            }
        }

        public static int GetSignedBits(int data, int pos, int bits)
        {
            int chop = 32 - bits;
            return (data << (chop - pos)) >> chop;
        }

        public static long SetBits(long data, int pos, int bits, long val)
        {
            var mask = (1L << bits) - 1;
            mask <<= pos;
            val <<= pos;
            return (data & ~mask) | (val & mask);
        }

        public static int SetBits(int data, int pos, int bits, int val)
        {
            var mask = (1 << bits) - 1;
            mask <<= pos;
            val <<= pos;
            return (data & ~mask) | (val & mask);
        }
        #endregion

		#region Errors

		public static void Warning(string message, params object[] args)
		{
			Error(ConsoleColor.Yellow, message, args);
		}

		public static void Info()
		{
			Info("");
		}

		public static void Info(string message, params object[] args)
		{
			Error(ConsoleColor.Cyan, message, args);
		}

		public static void Error(string message, params object[] args)
		{
			Error(ConsoleColor.Red, message, args);
		}

		public static void Error(ConsoleColor color, string message, params object[] args)
		{
#if !MONO
			var oldColor = Console.ForegroundColor;
			Console.ForegroundColor = color; 
#endif
			Console.Error.WriteLine(StringTools.Format(message, args));

#if !MONO	
			Console.ForegroundColor = oldColor;
#endif
		}

		#endregion

		#region Debugging

		public static bool? IsBrowsable(MemberInfo info)
		{
			var attr = info.GetCustomAttribute<BrowsableAttribute>();
			if (attr == null) return null;
			return attr.Browsable;
		}

		public static string GetDescription(MemberInfo info)
		{
			var attr = info.GetCustomAttribute<DescriptionAttribute>();
			if (attr == null) return null;
			return attr.Description;
		}

		#endregion
	}

}