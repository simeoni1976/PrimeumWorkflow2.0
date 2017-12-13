using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Transverse.Environment
{
    public class LoggingEvents
    {
        /// <summary>
        /// Main events
        /// </summary>
        public const int GENERATE_ITEMS = 1000;
        public const int LIST_ITEMS = 1001;
        public const int GET_ITEM = 1002;
        public const int INSERT_ITEM = 1003;
        public const int UPDATE_ITEM = 1004;
        public const int DELETE_ITEM = 1005;
        //..
        public const int GET_ITEM_NOTFOUND = 4000;
        public const int UPDATE_ITEM_NOTFOUND = 4001;

        /// <summary>
        /// Critical error : can't continue, must stop treatment.
        /// </summary>
        public const int CRITICAL_ERROR = 1;

        /// <summary>
        /// Process error : can't continue, must stop treatment.
        /// </summary>
        public const int PROCESS_ERROR = 2;

        /// <summary>
        /// Un problème est survenu : vérifier les paramètres d'entrées.
        /// </summary>
        public const int WARNING_ERROR = 4;
    }
}
