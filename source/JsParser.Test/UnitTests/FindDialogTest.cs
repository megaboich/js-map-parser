using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using JsParser.UI.UI;

namespace JsParserTest.UnitTests
{
	[TestFixture]
	public class FindDialogTest
	{
		[Test]
		public void TestFindFunction()
		{
			ValidateSplitting(
				new[] {
					new[]{"TestFunction", "Test | Function"},
					new[]{"TestCreepyFunction", "Test | Creepy | Function"},
					new[]{"TestFunkyFunction", "Test | Funky | Function"},
					new[]{"LongFunctionTestName", "Long | Function | Test | Name"},
					new[]{"LongSuperDuperFunctionName", "Long | Super | Duper | Function | Name"},
					new[]{"longSuperDuperFunctionName", "long | Super | Duper | Function | Name"},
					new[]{"A", "A"},
					new[]{"AB", "A | B"},
					new[]{"ABC", "A | B | C"},
					new[]{"aBC", "a | B | C"},
					new[]{"AaBbCc", "Aa | Bb | Cc"},
					new[]{"aaBbCc", "aa | Bb | Cc"},
					new[]{"A1B2C3", "A | 1 | B | 2 | C | 3"},
					new[]{"A123", "A | 1 | 2 | 3"},
					new[]{"123", "1 | 2 | 3"},
					new[]{"1A2B3C", "1 | A | 2 | B | 3 | C"},
					new[]{"1a2b3c", "1a | 2b | 3c"},
			});

			ValidateSplitting(
				new[] {
					new[]{"long*DuperFunctionName", "long | * | Duper | Function | Name"},
					new[]{"long*duperFunctionName", "long | * | duper | Function | Name"},
					new[]{"*duperFunctionName", "* | duper | Function | Name"},
					new[]{"duperFunctionName*", "duper | Function | Name | *"},
			});

			//Check match
			ValidateMatch(
				"AB",
				new[] {"aaBbCc",
						"ABC",
						"AB"
				},
				true);

			//Check match
			ValidateMatch(
				"*AB",
				new[] {
					"xxAaBb",
					"yABD",
					"yZAB",
					"yZWAB",
					"yZWRAB",
				},
				true);

			//Check not match
			ValidateMatch(
				"ABD",
				new[] {
					"aaBbCc",
					"A",
					"CBD",
					"AB",
				},
				false);

			//Check not match
			ValidateMatch(
				"*IB",
				new[] {
					"VeryCreepyNameFunction",
					"A",
					"BD",
					"AB",
				},
				false);
		}

		private void ValidateMatch(string pattern, string[] fnames, bool isMatch)
		{
			if (isMatch)
			{
				fnames.ToList().ForEach(p => { var r = TestMatch(pattern, p); if (!r) Assert.Fail("Pattern not match: " + pattern + "=>" + p); });
			}
			else
			{
				fnames.ToList().ForEach(p => { var r = TestMatch(pattern, p); if (r) Assert.Fail("Pattern match: " + pattern + "=>" + p); });
			}
		}

		private bool TestMatch(string pattern, string fname)
		{
			var p = FindDialog.SplitFunctionName(pattern).ToList();
			var f = FindDialog.SplitFunctionName(fname).ToList();
			return FindDialog.CompareEntities(p, f);
		}

		private void ValidateSplitting(string[][] input)
		{
			input.ToList().ForEach(p => Assert.AreEqual(p[1], FindDialog.GetFunctionNameTestTransform(p[0])));
		}
	}
}
