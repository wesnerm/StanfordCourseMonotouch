#region Using

/////////////////////////////////////////////////////////////////////////////
// This source code may not be reviewed, copied, or redistributed without
// the expressed permission of Wesner Moise.
// 
// Copyright (C) 2002-2010, Wesner Moise.
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace WM
{
	/// <summary>
	/// Summary description for StringTools.
	/// </summary>
	public static class StringTools
	{
		[DebuggerStepThrough]
		public static string CamelCase(string s)
		{
			return PascalCase(s, false);
		}

		//[DebuggerStepThrough]
		//public static string PascalCase(this string s, bool upCase = true)
		//{
		//    var b = new StringBuilder(s.Length);
		//    // TODO: Handle already joined
		//    for (int read = 0; read < s.Length; read++)
		//    {
		//        char ch = s[read];
		//        if (char.IsLetter(ch))
		//        {
		//            b.Append(upCase ? char.ToUpper(ch) : char.ToLower(ch));
		//            upCase = false;
		//        }
		//        else 
		//        {
		//            upCase = true;
		//            if (char.IsDigit(ch))
		//            {
		//                if (read == 0)
		//                    b.Append('_');
		//                b.Append(ch);
		//            }
		//        }
		//    }
		//    return b.ToString();
		//}

		[DebuggerStepThrough]
		public static string PascalCase(this string s, bool upCase = true)
		{
			var sb = new StringBuilder(s.Length);
			for (int i = 0; i < s.Length; )
			{
				char ch = s[i];
				int start = i++;
				if (!Char.IsLetterOrDigit(ch))
					continue;

				if (Char.IsDigit(ch))
				{
					if (start == 0)
						sb.Append('_');
					sb.Append(ch);
					while (i < s.Length && Char.IsDigit(s, i))
						sb.Append(s[i++]);

					upCase = true;
					continue;
				}

				sb.Append(upCase ? char.ToUpper(ch) : char.ToLower(ch));
				upCase = true;

				if (Char.IsUpper(ch))
				{
					while (i < s.Length && Char.IsUpper(s, i))
						sb.Append(s[i++]);

					if (i - start > 1 && i < s.Length && Char.IsLower(s, i))
					{
						i--;
						sb.Length--;
					}
					else
					{
						while (i < s.Length && Char.IsLower(s, i))
							sb.Append(s[i++]);
					}
				}
				else
				{
					while (i < s.Length)
					{
						var ch2 = s[i];
						if (Char.IsLower(ch2))
							sb.Append(ch2);
						else if (ch2 != '\'')
							break;
						i++;
					}
				}
			}
			return sb.ToString();
		}

		[DebuggerStepThrough]
		public static string TitleCase(string s)
		{
			bool upCase = true;
			var b = new StringBuilder(s);
			int write = 0;
			foreach (char ch in s)
			{
				if (char.IsLetter(ch))
				{
					b[write++] = upCase 
						? char.ToUpper(ch) 
						: char.ToLower(ch);
					upCase = false;
				}
				else
				{
					upCase = true;
					b[write++] = ch;
				}
			}
			return b.ToString(0, write);
		}

		public static string ToLowerFirst(this string s)
		{
			if (string.IsNullOrEmpty(s) || !char.IsUpper(s, 0))
				return s;
			return char.ToLower(s[0]) + s.Substring(1);
		}

		public static string ToUpperFirst(this string s)
		{
			if (string.IsNullOrEmpty(s) || !char.IsLower(s, 0))
				return s;
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public static bool IsNullOrEmpty(this string s)
		{
// ReSharper disable ReplaceWithStringIsNullOrEmpty
			return s == null || s.Length == 0;
// ReSharper restore ReplaceWithStringIsNullOrEmpty
		}

		public static bool IsNonempty(this string s)
		{
			// ReSharper disable ReplaceWithStringIsNullOrEmpty
			return s != null && s.Length > 0;
			// ReSharper restore ReplaceWithStringIsNullOrEmpty
		}

		public static string Chomp(this string line)
		{
			int len = line.Length;
			while (len > 0)
			{
				int ch = line[len - 1];
				if (ch != '\n' && ch != '\r')
					break;
				len--;
			}
			return line.Substring(0, len);
		}

		public static string Format(string message, params object[] args)
		{
			if (args == null || args.Length == 0)
				return message;
			return string.Format(message, args);
		}

		public static unsafe void Set(string str, int position, string replace)
		{
			int len = str.Length;
			int maxLen = len - position;

			if (maxLen < replace.Length)
				replace = replace.Substring(0, maxLen);

			fixed (char* p = str)
			fixed (char* r = replace)
			{
				char* pWrite = p + position;
				char* pRead = r;
				int replaceLen = replace.Length;
				while (replaceLen-- > 0)
					*pWrite++ = *pRead++;
			}
		}

		public static bool IndexOfAny(string str, out IndexResult result,
		                              params string[] list)
		{
			int len = str.Length;
			int count = list.Length;

			result = new IndexResult();
			int whichLength = -1;
			int which = -1;
			int index = len;

			for (int i = 0; i < count; i++)
			{
				string test = list[i];
				int testLength = test.Length;

				int cap = index + testLength;
				if (cap > len) cap = len;

				int pos = str.IndexOf(test, 0, cap);
				if (pos > -1 && pos < index
				    || pos == index && testLength > whichLength)
				{
					index = pos;
					which = i;
					whichLength = testLength;
				}
			}

			result.Found = which >= 0 ? list[which] : null;
			result.Index = index;
			result.Which = which;
			result.Length = whichLength;

			return which > 0;
		}

		public static void Split(string src, 
			out string first, out string second, out string third, char sep)
		{
			string tmp;
			Split(src, out first, out tmp, sep);
			Split(tmp, out second, out third, sep);
		}

		public static List<string> Scan(string data, string format)
		{
			int len = format.Length;
			int lenData = format.Length;
			int iData = 0;
			List<string> result = null;

			for (int i = 0; i < len && iData < lenData; i++)
			{
				char ch = format[i];
				if (ch == '%')
				{
					bool ignore = false;

					// Skip first character
					if (++i == len) break;
					ch = format[i];
					if (ch == '%') goto Test;

					// Examine remaining characters
					if (ch == '*')
					{
						ignore = true;
						if (++i == len) break;
						ch = format[i];
					}

					// Get length
					int maxLen = 0;
					while (ch >= '0' && ch <= '9')
					{
						if (++i == len) break;
						maxLen = maxLen*10 + ch - '0';
						ch = format[i];
					}
					if (i >= len) break;

					object o;
					int start = iData;
					switch (char.ToLower(ch))
					{
					case 'c':
						if (maxLen == 0) maxLen = 1;
						while (iData < lenData && maxLen-- > 0)
							iData++;
						o = data.Substring(start, iData - start);
						break;
					case 's':
						if (data[iData] == '"')
						{
							if (maxLen == 0) maxLen = int.MaxValue;
							while (iData < lenData
							       && maxLen-- != 0
							       && data[iData] != '"')
								iData++;
							o = data.Substring(start + 1, iData - start - 1);
							if (iData < lenData) iData++;
						}
						else
						{
							while (iData < lenData && !char.IsWhiteSpace(data, iData))
								iData++;
							o = data.Substring(start, iData - start);
						}
						break;
					case 'd':
						while (iData < lenData && "1234567890-+".IndexOf(ch) != -1)
							iData++;
						o = Convert.ToInt32(data.Substring(start, iData - start));
						break;
					case 'x':
						while (iData < lenData && "1234567890abcdefABCDEF".IndexOf(ch) != -1)
							iData++;
						o = Convert.ToInt32(data.Substring(start, iData - start), 16);
						break;
					case 'f':
						while (iData < lenData && "1234567890.e+-".IndexOf(ch) != -1)
							iData++;
						o = Convert.ToDouble(data.Substring(start, iData - start));
						break;
					case 'm':
						while (iData < lenData && "$,.1234567890+-".IndexOf(ch) != -1)
							iData++;
						o = Convert.ToDouble(data.Substring(start, iData - start));
						break;
					default:
						throw (new FormatException());
					}

					if (!ignore)
					{
						if (result == null)
							result = new List<string>();
						result.Add((string) o);
					}
				}

				if (ch == ' ')
				{
					while (iData < lenData && char.IsWhiteSpace(data[iData]))
						iData++;
					continue;
				}

				Test:
				// If character doesn't match
				if (ch != data[iData])
					break;
				iData++;
			}

			return result;
		}

		public static string Join(string middle, IEnumerable enumerable)
		{
			return Join(enumerable, middle);
		}

		public static string Join(IEnumerable enumerable, 
			string middle=null,
			string before="", 
			string after="")
		{
			var builder = new StringBuilder();
			builder.Append(before);
			bool first = true;

			if (middle == null)
				middle = enumerable is IEnumerable<char> ? "" : ", ";

			foreach (object o in enumerable)
			{
				if (first)
					first = false;
				else
					builder.Append(middle);
				builder.Append(o.ToString());
			}
			builder.Append(after);
			return builder.ToString();
		}


		public static void Trim(string[] array)
		{
			int len = array.Length;
			for (int i = 0; i < len; i++)
				array[i] = array[i].Trim();
		}

		public static bool EndsWith(this StringBuilder sb, string text)
		{
			if (text == null)
				return true;

			var offset = sb.Length - text.Length;
			if (offset < 0)
				return false;

			for(int i=text.Length-1; i>=0; i--)
			{
				if (text[i] == sb[offset+i])
					return true;
			}

			return true;
		}

		public static StringBuilder ToUpper(StringBuilder buffer)
		{
			int len = buffer.Length;
			for (int i = 0; i < len; i++)
				buffer[i] = char.ToUpper(buffer[i]);
			return buffer;
		}

		public static StringBuilder ToLower(StringBuilder buffer)
		{
			int len = buffer.Length;
			for (int i = 0; i < len; i++)
				buffer[i] = char.ToLower(buffer[i]);
			return buffer;
		}

		public static StringBuilder Trim(StringBuilder buffer)
		{
			TrimLeft(buffer);
			TrimRight(buffer);
			return buffer;
		}

		public static StringBuilder TrimLeft(StringBuilder buffer)
		{
			int len = buffer.Length;
			int i;

			for (i = 0; i < len; i++)
				if (!char.IsWhiteSpace(buffer[i]))
					break;

			if (i > 0)
				buffer.Remove(0, i);

			return buffer;
		}

		public static StringBuilder TrimRight(StringBuilder buffer)
		{
			int len = buffer.Length;

			while (len > 0 && char.IsWhiteSpace(buffer[len - 1]))
				len--;

			buffer.Length = len;
			return buffer;
		}

		public static string[] Split(string line, char separator)
		{
			line = Chomp(line).Trim();

			int i = 0;
			int len = line.Length;
			int start = 0;
			var array = new List<string>();
			bool quote = false;

			while (i < len)
			{
				int ch = line[i];
				if (ch == separator && !quote)
				{
					if (i != start || separator != ' ')
						array.Add(line.Substring(start, i - start));
					start = i + 1;
				}
				else if (ch == '"')
				{
					if (i == start)
						quote = true;
					else if (quote != true)
					{
						// SKIP
					}
					else
					{
						quote = false;
						start++;
						array.Add(line.Substring(start, i - start));
						start = i + 1;
					}
				}
				i++;
			}

			if (i != start || separator != ' ')
				array.Add(line.Substring(start, i - start));

			return array.ToArray();
		}

		public static int Compare(StringBuilder s1, string s2)
		{
			int len1 = s1.Length;
			int len2 = s2.Length;

			int len = len1 < len2 ? len1 : len2;
			int cmp = Compare(s1, 0, s2, 0, len);
			if (cmp == 0) return len1 - len2;
			return cmp;
		}

		public static int Compare(StringBuilder s1, int start1, StringBuilder s2, int start2, int length)
		{
			while (length-- > 0)
			{
				int cmp = s1[start1++] - s2[start2++];
				if (cmp != 0) return cmp;
			}
			return 0;
		}

		public static int Compare(StringBuilder s1, int start1, string s2, int start2, int length)
		{
			while (length-- > 0)
			{
				int cmp = s1[start1++] - s2[start2++];
				if (cmp != 0) return cmp;
			}
			return 0;
		}

		public static string Repeat(string data, int count)
		{
			if (count == 1)
				return data;
			if (count <= 0 || data.Length==0)
				return "";
			var sb = new StringBuilder(data.Length*count);
			for (int i = 0; i < count; i++)
				sb.Append(data);
			return sb.ToString();
		}

		public static string Reverse(this string s)
		{
			if (s.Length < 2)
				return s;

			var sb = new StringBuilder(s);
			var last = s.Length - 1;
			for (int i = s.Length/2-1; i >=0; i--)
			{
				var tmp = sb[i];
				sb[i] = sb[last - i];
				sb[last - i] = tmp;
			}
			return sb.ToString();
		}

		public static string Indent(this string message, string tab="\t", int indent = 1)
		{
			tab = Repeat(tab, indent);
			var sb = new StringBuilder(message.Length + tab.Length + 6);
			if (!message.StartsWith("\n")) sb.Append(tab);
			sb.Append(message);
			sb.Replace("\n", "\n" + tab);
			return sb.ToString();
		}

		public static int Dec(string number)
		{
			return Convert.ToInt32(number, 16);
		}

		public static string Hex(this int number)
		{
			return number.ToString("x");
		}

		public static bool IsValid(string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		public static string Wrap(string text, int columns = 65)
		{
			if (text.Length <= columns)
				return text;

			var sb = new StringBuilder();
			int read = 0;
			int write = 0;
			int lastSpace = -1;

			while (read<text.Length)
			{
				char ch = text[read];
				sb.Append(ch);

				if (read - write > columns || ch == '\n')
				{
					if (lastSpace > write && ch != '\n')
						read = lastSpace;

					sb.Append(text, write, read - write);
					sb.AppendLine();

					while (read<text.Length)
					{
						char c2 = text[read];
						if (!char.IsWhiteSpace(c2))
							break;
						if (c2 == '\n')
						{
							read++;
							break;
						}
					}

					write = read;
					continue;
				}

				if (char.IsWhiteSpace(ch))
					lastSpace = read;

				read++;
			}

			sb.Append(text, write, read - write);
			return sb.ToString();
		}

		public static StringBuilder Join<T>(
			this StringBuilder b,
			IEnumerable<T> collection,
			string join = " ",
 			Func<T,string> selector = null)
		{
			bool rest = false;
			foreach(var item in collection)
			{
				if (rest)
				{
					if (!string.IsNullOrEmpty(join))
						b.Append(join);
				}
				else
				{
					rest = true;
				}

				var message = selector != null ? selector(item) : item.ToString();
				b.Append(message);
			}
			return b;
		}

		[DebuggerStepThrough]
		public static bool Split(string s, char ch, out string before, out string after)
		{
			int index = s.IndexOf(ch);
			if (index < 0)
			{
				before = s;
				after = "";
				return false;
			}

			before = s.Substring(0, index);
			after = s.Substring(index + 1);
			return true;
		}

		[DebuggerStepThrough]
		public static string Before(this string s, char match, 
			string defaultResult = null)
		{
			int index = s.IndexOf(match);
			if (index < 0)
				return defaultResult;
			return s.Substring(0, index);
		}

		[DebuggerStepThrough]
		public static string BeforeLast(this string s, char match,
			string defaultResult = null)
		{
			int index = s.LastIndexOf(match);
			if (index < 0)
				return defaultResult;
			return s.Substring(0, index);
		}

		[DebuggerStepThrough]
		public static string AfterLast(this string s, char match, string defaultResult = null)
		{
			int index = s.LastIndexOf(match);
			if (index < 0)
				return defaultResult;
			return s.Substring(index + 1);
		}

		[DebuggerStepThrough]
		public static string After(this string s, char match, string defaultResult = null)
		{
			int index = s.IndexOf(match);
			if (index < 0)
				return defaultResult;
			return s.Substring(index + 1);
		}

		[DebuggerStepThrough]
		public static string PrefixByteOrderMark(string s)
		{
			byte[] preamble = Encoding.Unicode.GetPreamble();
			String byteOrderMark = Encoding.Unicode.GetString(preamble, 0, preamble.Length);
			return byteOrderMark + s;
		}

		[DebuggerStepThrough]
		public static bool CheckStart(ref string s, string prefix, bool caseSensitive)
		{
			if (s.StartsWith(prefix, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
			{
				s = s.Substring(prefix.Length);
				return true;
			}
			return false;
		}

		[DebuggerStepThrough]
		public static bool CheckEnd(ref string s, string suffix, bool caseSensitive)
		{
			if (s.EndsWith(suffix, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
			{
				s = s.Substring(0, s.Length - suffix.Length);
				return true;
			}
			return false;
		}


		[DebuggerStepThrough]
		public static string Normalize(string s, bool removeSpaces = true)
		{
			var b = new StringBuilder(s);

			int write = 0;
			for (int read = 0; read < s.Length; read++)
			{
				char ch = s[read];
				if (removeSpaces && (ch < '0' || ch == '_'))
					continue;
				ch = char.ToLower(ch);
				b[write++] = ch;
			}

			return b.ToString(0, write);
		}

		[DebuggerStepThrough]
		public static int EditDistance(string source, string target)
		{
			const int Max = 100;

			// Get Lengths
			if (source.Length == 0) return target.Length;
			if (target.Length == 0) return source.Length;

			if (source.Length > Max || target.Length > Max)
				return source.Length + target.Length;

			// Initialize array
			var distance = new byte[source.Length + 1,target.Length + 1];
			for (int i = 0; i <= source.Length; i++)
				distance[i, 0] = (byte) i;
			for (int j = 0; j <= target.Length; j++)
				distance[0, j] = (byte) j;

			// Perform edit distance tests
			for (int i = 0; i < source.Length; i++)
			{
				char ch1 = source[i];
				for (int j = 0; j < target.Length; j++)
				{
					char ch2 = target[j];
					int d = distance[i, j];
					if (ch1 != ch2) d += 1;
					d = Math.Min(d, distance[i + 1, j] + 1); // deletion
					d = Math.Min(d, distance[i, j + 1] + 1); // insertion
					distance[i + 1, j + 1] = (byte) Math.Min(255, d);
				}
			}
			return distance[source.Length, target.Length];
		}

		#region Index

		[DebuggerStepThrough]
		public static int IndexOf(this StringBuilder text, char findChar, int start = 0)
		{
			return text.IndexOf(findChar, start, text.Length - start);
		}

		[DebuggerStepThrough]
		public static int IndexOf(this StringBuilder text, char findChar, int start, int count)
		{
			while (count-- > 0)
			{
				if (text[start] == findChar)
					return start;
				start++;
			}
			return -1;
		}

		[DebuggerStepThrough]
		public static int IndexOf(this StringBuilder text, string findText, int start = 0)
		{
			return IndexOf(text, findText, start, text.Length - start);
		}

		[DebuggerStepThrough]
		public static int IndexOf(this StringBuilder text, string findText, int start, int count)
		{
			int textLen = text.Length;
			int findLen = findText.Length;

			if (start > textLen - count)
				count = textLen - start;

			count -= findLen;
			while (count-- >= 0)
			{
				int i;
				for (i = 0; i < findText.Length; i++)
					if (text[start + i] != findText[i])
						break;
				if (i == findText.Length) 
					return start;
				start++;
			}

			return -1;
		}

		[DebuggerStepThrough]
		public static int LastIndexOf(this StringBuilder b, string text)
		{
			int currentIndex = -1;
			while (true)
			{
				if (currentIndex + text.Length > b.Length)
					return currentIndex;

				int newIndex = b.IndexOf(text, currentIndex + 1);
				if (newIndex < 0)
					return currentIndex;

				currentIndex = newIndex;
			}
		}


		#endregion

		#region Split

		public static string Split(string src, out string left, out string right, params string[] sep)
		{
			IndexResult result;
			if (!IndexOfAny(src, out result, sep))
			{
				left = src;
				right = String.Empty;
				return null;
			}

			int index = result.Index;
			left = src.Substring(0, index);
			right = src.Substring(index + 1);
			return result.Found;
		}

		public static bool Split(string src, out string left, out string right, string sep)
		{
			int index = src.IndexOf(sep);
			if (index == -1)
			{
				left = src;
				right = String.Empty;
				return false;
			}

			left = src.Substring(0, index);
			right = src.Substring(index + sep.Length);
			return true;
		}

		public static char Split(string src, out string left, out string right, params char[] sep)
		{
			int index = src.IndexOfAny(sep);
			if (index == -1)
			{
				left = src;
				right = String.Empty;
				return '\0';
			}

			left = src.Substring(0, index);
			right = src.Substring(index + 1);
			return src[index];
		}

		public static bool Split(string src, out string left, out string right, char sep)
		{
			int index = src.IndexOf(sep);
			if (index == -1)
			{
				left = src;
				right = String.Empty;
				return false;
			}

			left = src.Substring(0, index);
			right = src.Substring(index + 1);
			return true;
		}

		public static bool SplitLast(string src, out string left, out string right, char sep)
		{
			int index = src.LastIndexOf(sep);
			if (index == -1)
			{
				left = src;
				right = String.Empty;
				return false;
			}

			left = src.Substring(0, index);
			right = src.Substring(index + 1);
			return true;
		}

		#endregion

		#region String Functions

		public static string Left(this string s, int count)
		{
			int len = s.Length;
			if (count > len) count = len;
			return s.Substring(0, count);
		}

		public static string Right(this string s, int count)
		{
			int len = s.Length;
			if (count > len) count = len;
			return s.Substring(len - count, count);
		}

		public static string Mid(this string s, int start)
		{
			int len = s.Length;
			if (len == 0) return s;
			if (start >= len) return string.Empty;
			return s.Substring(start);
		}

		public static string Mid(this string s, int start, int count)
		{
			int len = s.Length;
			if (start >= len) return string.Empty;
			if (start + count > len) count = len - start;
			return s.Substring(start, count);
		}

		#endregion

		#region Normalization

		/// <summary>
		/// Reduces all linebreaks to '\r'
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
// ReSharper disable InconsistentNaming
		public static string NormalizeCRLF(string text)
// ReSharper restore InconsistentNaming
		{
			if (text == null) return "";

			int index = text.IndexOf('\n');
			if (index == -1)
				return text;

			var sb = new StringBuilder(text.Length);

			for (int i = 0; i < text.Length; i++)
			{
				char ch = text[i];
				if (ch == '\n')
				{
					if (i == 0 || text[i - 1] != '\r')
						sb.Append('\r');
				}
				else
					sb.Append(ch);
			}

			return sb.ToString();
		}

		#endregion

		#region Bump
		
		public static long RemoveNumber(StringBuilder builder)
		{
			int value = 0;
			int index = builder.Length-1;
			int start = Math.Max(0,builder.Length - 18);

			while (index >= start)
			{
				char ch = builder[index];
				if (ch < '0' || ch > '9')
					break;
				
				value = value * 10 + ch;
				index--;
			}

			index++;
			if (builder.Length == index)
				return 0;

			builder.Length = index;
			return value;
		}

		public static string Bump(string s, bool underscore = false)
		{
			var builder = new StringBuilder(s);
			var number = RemoveNumber(builder);
			number = number + 1;

			if (underscore)
			{
				int length = builder.Length;
				if (length <= 0 || builder[length - 1] != '_')
				{
					builder.Append('_');
					number = 0;
				}
			}

			if (number <= 0)
				number = 2;

			builder.Append(number);
			return builder.ToString();
		}

		public static int HexDigit(int ch)
		{
			if ((uint)(ch - '0') < 10)
				return ch - '0';
			if ((uint)(ch - 'a') < 6)
				return ch - 'a' + 10;
			if ((uint)(ch - 'A') < 6)
				return ch - 'A' + 10;
			return -1;
		}

		public static int Digit(int ch)
		{
			unchecked
			{
				int digit = (ch - '0');
				if ((uint)digit <= 9)
					return digit;
				return -1;
			}
		}

		public static bool IsLetter(int ch)
		{
			unchecked
			{
				var ch2 = (uint)(ch - 'A');
				return (ch2 <= ('z' - 'A')) 
					&& ((ch2 & ~32) <= ('Z' - 'A'));
			}
		}

		public static bool IsDigit(int ch)
		{
			unchecked
			{
				return unchecked((uint) (ch - '0') <= 9);
			}
		}

		public static bool IsWord(int ch)
		{
			if (ch < 'A')
				return IsDigit(ch);

			if (ch <= 'Z')
				return true;

			if (ch < 'a')
				return ch == '_';

			return ch <= 'z';
		}


		public static bool IsWordUnicode(int ch)
		{
			unchecked
			{
				return char.IsLetterOrDigit((char) ch) || ch == '_';
			}
		}

		#endregion
		
		#region Nested type: IndexResult

		public struct IndexResult
		{
			public string Found;
			public int Index;
			public int Length;
			public int Which;
		}

		#endregion

		#region PrintF

		public static string PrintF(this string format, 
			params object[] args)
		{
			var sb = new StringBuilder();
			int i = 0;
			int arg = 0;

			while (i < format.Length)
			{
				int index = format.IndexOf('%', i);
				if (index < 0)
				{
					sb.Append(format, i, format.Length - i);
					break;
				}

				sb.Append(format, i, index-i);
				i = index + 1;
				if (i < format.Length && format[i] == '%')
				{
					sb.Append('%');
					continue;
				}

				bool quit = false;
				bool dec = false;
				bool rightAlign = true;
				int width = 0;
				int precision = 0;
				char pad = ' ';
				object data = args[arg++] ?? "(null)";
				string plus = null;
				for (; i < format.Length; i++)
				{
					char ch = format[i];
					switch (ch)
					{
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						int c = ch - '0';
						if (dec)
							precision = precision * 10 + c;
						else
						{
							width = width * 10 + c;
							if (width == 0)
								pad = '0';
						}
						break;
					case 'l':
					case 'L':
					case 'h':
						break;
					case '.':
						if (dec)
							throw new FormatException();
						dec = true;
						break;
					case '*':
						c = Convert.ToInt32(data);
						data = args[arg++] ?? "(null)";
						if (dec)
							precision = c;
						else
							width = c;
						break;
					case '-':
						rightAlign = false;
						break;
					case '+':
						plus = "+";
						break;
					case ' ':
						plus = " ";
						break;
					case 'c':
						if (!dec)
							data = Convert.ToChar(data);
						else
						{
							var cdata = data as char[];
							if (cdata != null)
								data = new string(cdata, 0,
									cdata.Length);
						}
						goto case 's';
					case 'q':
					case 'Q':
						data = EncodeSql(data.ToString(),
										 ch == 'Q');
						goto case 's';
					case 's':
					case 'p':
						var s = data.ToString();
						if (dec && s.Length > precision)
							s = s.Remove(precision);
						data = s;
						quit = true;
						break;
					case 'i':
					case 'u':
						ch = 'D';
						goto case 'D';
					case 'd':
					case 'D':
					case 'e':
					case 'E':
					case 'g':
					case 'G':
					case 'f':
					case 'F':
					case 'x':
					case 'X':
						var d = data;
						var iformat = data as IFormattable;
						if (iformat != null)
						{
							var f = ch.ToString();
							if (dec)
								f += precision;
							data = iformat.ToString(f, null);
						}
						if (plus != null && Convert.ToDouble(d) > 0)
							data = plus + data;
						quit = true;
						break;
					default:
						throw new FormatException();
					}

					if (quit)
					{
						string result = data.ToString();
						if (width > result.Length)
						{
							result = rightAlign
										? result.PadLeft(width, pad)
										: result.PadRight(width);
						}
						sb.Append(result);
						break;
					}
				}

				if (i == format.Length)
					throw new FormatException();

				i++;
			}

#if DEBUG
			if (arg < args.Length)
				Debugger.Break();
#endif

			return sb.ToString();
		}

		public static ulong? ParseInteger(string text, ref int i)
		{
			int j = i;
			ulong number = 0;

			for (; j < text.Length; j++ )
			{
				var ch = text[j];
				if (ch < '0' || ch > '9') break;
				var digit = (ulong)(ch - '0');
				var oldnumber = number;
				number = unchecked(number * 10 + digit);
				if (number < oldnumber)
					return null;
			}

			if (i == j)
				return null;

			i = j;
			return number;
		}


		public static string EncodeSql(this string s, 
			bool quoteFully = true)
		{
			var sb = new StringBuilder(s);
			sb.Replace("'", "''");
			if (quoteFully)
			{
				sb.Insert(0, '\'');
				sb.Append('\'');
			}
			return sb.ToString();
		}

		public static string EncodeHtml(this string s)
		{
			throw new NotImplementedException();
		}

		public static string EncodeUrl(this string s)
		{
			throw new NotImplementedException();
		}
		#endregion

        #region Diff

        public static bool SimpleDiff(string string1, string string2,
            out int start, out int charsFromEnd, bool ignoreCase = false)
        {
            int s;
            int e;

            start = 0;
            charsFromEnd = 0;

            if (string1 == null
                || string2 == null
                || string1.Length == 0
                || string2.Length == 0)
                return false;

            int last1 = string1.Length - 1;
            int last2 = string2.Length - 1;
            int count = Math.Min(last1, last2) + 1;
            if (count == 0)
                return false;

            for (s = 0; s < count; s++)
            {
                char ch1 = string1[s];
                char ch2 = string2[s];
                if (ch1 != ch2
                    || ignoreCase && IsSameLetter(ch1, ch2))
                    break;
            }

            count = Math.Min(count, count - s);
            for (e = 0; e < count; e++)
            {
                char ch1 = string1[last1 - e];
                char ch2 = string2[last2 - e];
                if (string1[last1 - e] != string2[last2 - e]
                    || ignoreCase && IsSameLetter(ch1, ch2))
                    break;
            }

            start = s;
            charsFromEnd = e;
            return true;
        }

        public static bool IsSameLetter(char ch1, char ch2)
        {
            return char.ToUpper(ch1) == char.ToUpper(ch2);
        }

        #endregion
    }
}