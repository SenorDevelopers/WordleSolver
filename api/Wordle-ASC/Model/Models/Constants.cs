namespace Model.Models;

public static class Constants
{
	public static readonly List<Pattern> ALL_PATTERNS = Pattern.GetAllPatterns();
	public const char CONTROL_CHAR = '0';
	public const int MAX_THREADS = 20;
	public const int DEPTH_SEARCH = 20;
	public const double MAX_PRECISION_ERROR = 0.001;
	public const int TOTAL_WORDS_COUNT = 11454;
	public const int WORD_SIZE = 5;
	public const int MAX_IN_DEPTH_GUESSES = 2;
}

