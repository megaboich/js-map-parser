using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParserCore.Parsers
{
    public static class TaskListAggregator
    {
        public static IEnumerable<TaskListItem> GetTaskList(IEnumerable<CustomComment> comments)
        {
            var targetComments = comments.Where(c => c.Text.IndexOf("TODO", StringComparison.InvariantCultureIgnoreCase) > 0);

            var taskList = targetComments.Select(c => new TaskListItem
                {
                    Description = c.Text,
                    StartLine = c.Start,
                    StartColumn = 0
                });

            return taskList;
        }
    }
}
