namespace BatchProcessor.Processing
{
    public enum ProcessingResult
    {
        /// <summary>
        /// Alles OK
        /// </summary>
        OK = 0,

        /// <summary>
        /// Ein unbehandelter Fehler ist aufgetreten
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Die Programm-Parameter wurden nicht korrekt übergeben
        /// </summary>
        InvalidArguments = 3,

        /// <summary>
        /// Die Konfigurations-Datei konnte nicht gefunden werden
        /// </summary>
        ConfigurationFileNotFound = 4
    }
}