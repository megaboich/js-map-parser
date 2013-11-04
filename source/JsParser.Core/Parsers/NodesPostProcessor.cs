namespace JsParser.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using JsParser.Core.Helpers;
    using JsParser.Core.Code;

    /// <summary>
    /// Process nodes hierarchy after parsing to improve structure
    /// </summary>
    public static class NodesPostProcessor
    {
        public static bool HideAnonymousFunctions(Hierachy<CodeNode> hierachy, JavascriptParserSettings settings)
        {
            if (settings.SkipAnonymousFuntions)
            {
                if (hierachy.Childrens != null && hierachy.Childrens.Any())
                {
                    hierachy.Childrens.RemoveAll(child => HideAnonymousFunctions(child, settings));
                }

                if (hierachy.Childrens != null && hierachy.Childrens.Any())
                {
                    return false;
                }

                if (hierachy.Item.AliasType == NodeAliasType.Anonymous)
                {
                    return true;
                }
            }

            return false;
        }

        public static void GroupNodesByVariableDeclaration(Hierachy<CodeNode> hierachy, JavascriptParserSettings settings)
        {
            do_stuff_again:
            if (hierachy.Childrens != null && hierachy.Childrens.Any())
            {
                if (settings.ProcessHierarchy)
                {
                    for (int rootCandidateIndex = 0; rootCandidateIndex < hierachy.Childrens.Count; ++rootCandidateIndex)
                    {
                        var rootCandidate = hierachy.Childrens[rootCandidateIndex];
                        var rootCandidateAlias = rootCandidate.Item.Alias;
                        var parenIndex = rootCandidateAlias.IndexOf('(');
                        if (parenIndex >= 0)
                        {
                            rootCandidateAlias = rootCandidateAlias.Substring(0, parenIndex);
                        }

                        rootCandidateAlias += ".";

                        for (int branchCandidateIndex = 0; branchCandidateIndex < hierachy.Childrens.Count; ++branchCandidateIndex)
                        {
                            if (rootCandidateIndex == branchCandidateIndex)
                            {
                                continue;
                            }

                            var branchCandidate = hierachy.Childrens[branchCandidateIndex];

                            if (branchCandidate.Item.Alias.StartsWith(rootCandidateAlias))
                            {
                                //Strip alias of sub-item
                                branchCandidate.Item.Alias = branchCandidate.Item.Alias.Substring(rootCandidateAlias.Length);

                                //Ensure that collection of childrens exists
                                rootCandidate.Childrens = rootCandidate.Childrens ?? new List<Hierachy<CodeNode>>();
                                //Add sub-item to new parent
                                rootCandidate.Childrens.Add(branchCandidate);

                                //Remove item from current collection
                                hierachy.Childrens.RemoveAt(branchCandidateIndex);

                                goto do_stuff_again;
                            }
                        }
                    }
                }

                // remove variables that do not contain sub-items
                hierachy.Childrens.RemoveAll(i => i.Item.Opcode == "Variable" && (i.Childrens == null || !i.Childrens.Any()));

                // run recursion
                foreach (var subHierarchy in hierachy.Childrens)
                {
                    GroupNodesByVariableDeclaration(subHierarchy, settings);
                }
            }
        }
    }
}
