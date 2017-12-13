namespace Workflow.Transverse.Environment
{
    /// <summary>
    /// Classe de constants. Voir pour la rendre configurable (depuis le startup, on chargerait une partie du fichier de conf dedans, en généralisant le process).
    /// </summary>
    sealed public class Constant
    {
        #region Paramétres divers

        /// <summary>
        /// Valeur MAX de la taille des messages dans les réponses du WebAPI BusinessCore.
        /// </summary>
        public const int RESPONSE_MAX_SIZE_MESSAGE = 2048;

        /// <summary>
        /// Timeout en secondes du temps d'attente du retour d'une réponse synchronisée.
        /// </summary>
        public const int SYNCRESPONSES_TIMEOUT = 60 * 15;

        /// <summary>
        /// Caractère séparateur dans les colonnes de dimension de type Tree.
        /// </summary>
        /// <remarks>Eviter les caractères trop courants pouvant être présent dans les identifiants de noeuds d'arbre (.,/^ etc...)</remarks>
        public const char DEFAULT_SEPARATOR_TREE = '.';

        /// <summary>
        /// Caractère de séparation pour les formats numériques
        /// </summary>
        public const char SEPARATOR_FORMAT = '|';


        /// <summary>
        /// Nom de la table de données des SelectorInstance.
        /// </summary>
        /// <remarks>La chaine doit être formatée (string.Format()) avec comme argument l'id du SelectorInstance.</remarks>
        public const string TEMPLATE_TEMPORARY_TABLENAME = "SelectorData_{0}";

        /// <summary>
        /// Chaine postfixe pour les noms des WorkflowConfig dupliqués.
        /// </summary>
        public const string POSTFIX_NAME_DUPLICATE_WORKFLOW_CONFIG = "{0} (Duplicate {1})";

        /// <summary>
        /// Regex pour controler et extraire le nom originale d'un WorkflowConfig dupliqué.
        /// </summary>
        public const string SUBSTRING_NAME_WORKFLOW_CONFIG = @"^(?<name>.+) \(Duplicate (?<number>[0-9]+)\)$";

        #endregion // Paramétres divers

        #region Nommage de variable de session

        public const string S_TRANSACTION_OBJECT = "session_transaction_object";

        public const string S_DBCONTEXT = "session_dbcontext";

        #endregion // Nommage de variable de session

        #region Noms de configuration

        /// <summary>
        /// Nom de la variable de conf pour le format numérique
        /// </summary>
        public const string CONFIGURATION_NAME_FORMAT = "NumericFormat";

        /// <summary>
        /// Nom pour la variable de conf pour le caractère de séparation des dimensions d'arbre
        /// </summary>
        public const string CONFIGURATION_NAME_SEPARATION_CHAR = "SeparatorTree";

        #endregion

        #region Nom des colonnes des dimensions (pour les requetes)
        /// <summary>
        /// Nom de la dimension 1
        /// </summary>
        public const string DATA_DIMENSION_1 = "Dimension1";
        /// <summary>
        /// Nom de la dimension 2
        /// </summary>
        public const string DATA_DIMENSION_2 = "Dimension2";
        /// <summary>
        /// Nom de la dimension 3
        /// </summary>
        public const string DATA_DIMENSION_3 = "Dimension3";
        /// <summary>
        /// Nom de la dimension 4
        /// </summary>
        public const string DATA_DIMENSION_4 = "Dimension4";
        /// <summary>
        /// Nom de la dimension 5
        /// </summary>
        public const string DATA_DIMENSION_5 = "Dimension5";
        /// <summary>
        /// Nom de la dimension 6
        /// </summary>
        public const string DATA_DIMENSION_6 = "Dimension6";
        /// <summary>
        /// Nom de la dimension 7
        /// </summary>
        public const string DATA_DIMENSION_7 = "Dimension7";
        /// <summary>
        /// Nom de la dimension 8
        /// </summary>
        public const string DATA_DIMENSION_8 = "Dimension8";
        /// <summary>
        /// Nom de la dimension 9
        /// </summary>
        public const string DATA_DIMENSION_9 = "Dimension9";
        /// <summary>
        /// Nom de la dimension 10
        /// </summary>
        public const string DATA_DIMENSION_10 = "Dimension10";

        /// <summary>
        /// Nom du champ TypeValue
        /// </summary>
        public const string DATA_TYPEVALUE = "TypeValue";

        #endregion // Nom des colonnes des dimensions (pour les requetes)


        #region Actions

        /// <summary>
        /// Nom de l'action hardcodé qui split les noeuds d'une secto d'un niveau donné sur les niveaux inférieurs.
        /// </summary>
        public const string ACTION_SPLIT_PRIMEUM = "SplitPrimeum";

        /// <summary>
        /// Nom de l'action hardcodé qui copie la valeurs des noeuds d'une secto d'un niveau donné sur les niveaux inférieurs.
        /// </summary>
        public const string ACTION_SPLIT_COPY = "SplitCopy";

        /// <summary>
        /// Nom de l'action hardcodé qui réagrége les noeuds d'une sector d'un niveau donné sur les niveaux supérieurs.
        /// </summary>
        public const string ACTION_AGREGATE_PRIMEUM = "AgregatePrimeum";



        #endregion // Actions

        #region Paramètres d'action

        /// <summary>
        /// Paramètre de l'action Split : nom de l'alignment sur lequel on split les valeurs modifiées.
        /// </summary>
        public const string PARAMETER_ACTION_SPLIT_PRIMEUM_ALIGNMENTNAME = "AlignmentName";

        /// <summary>
        /// Paramètre de l'action Aggregate : nom de l'alignment sur lequel on agrége les valeurs modifiées.
        /// </summary>
        public const string PARAMETER_ACTION_AGGREGATE_PRIMEUM_ALIGNMENTNAME = "AlignmentName";

        #endregion // Paramètres d'action

        #region Contraintes

        public const string CONSTRAINT_TREESUM = "TreeSum";

        #endregion // Contraintes

        #region Paramètres de contraintes

        public const string PARAMETER_CONSTRAINT_ALIGNMENTNAME = "AlignmentName";

        #endregion // Paramètres de contraintes
    }
}
