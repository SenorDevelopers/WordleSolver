using Database.Enums;

namespace Model.Models;

public class Pattern
{
	private List<MatchWays> _content;

	public List<MatchWays> Content
	{
		get => _content;
		set
		{
			if (IsValidPattern(value))
			{
				_content = value;
			}
			else
			{
				throw new Exception("Assigned an invalid content to a Pattern");
			}
		}
	}

	private const int PATTERN_LENGTH = 5;

	public Pattern()
	{
		_content = new List<MatchWays>()
		{
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist,
			MatchWays.NotExist
		};
	}

	public Pattern(string content)
	{
		if (!IsValidPattern(content))
		{
			throw new Exception("Try creating a pattern with and invalid content!");
		}

		var result = new List<MatchWays>();

		foreach (var chr in content)
		{
			switch (chr)
			{
				case ('0'):
					result.Add(MatchWays.NotExist);
					break;

				case ('1'):
					result.Add(MatchWays.ExistInWord);
					break
						;
				case ('2'):
					result.Add(MatchWays.ExistAtTheSamePosition);
					break;
			}
		}

		_content = result;
	}

	public Pattern(List<MatchWays> content)
	{
		if (!IsValidPattern(content))
		{
			throw new Exception("Try creating a pattern with and invalid content!");
		}

		_content = content;
	}

	public override string ToString()
	{
		var ans = "";

		foreach (var way in _content)
		{
			var indexOfTheWayInEnum = ((int)way).ToString();
			ans += indexOfTheWayInEnum;
		}

		return ans;
	}

	private static bool IsValidPattern(ICollection<MatchWays> pattern)
	{
		return pattern.Count == PATTERN_LENGTH;
	}

	private bool IsValidPattern(string pattern)
	{
		foreach (var chr in pattern)
		{
			if (chr is > '2' or < '0')
			{
				return false;
			}
		}

		return pattern.Length == PATTERN_LENGTH;
	}

	public static List<Pattern> GetAllPatterns()
	{
		var patterns = new List<Pattern>();
		var currentState = new Pattern();

		GenerateAllPatterns(ref patterns, PATTERN_LENGTH, 0, ref currentState);

		return patterns;
	}

	private static void GenerateAllPatterns(ref List<Pattern> patterns, int length, int currentLength,
		ref Pattern currentPattern)
	{
		if (currentLength == length)
		{
			var newPattern = new Pattern(currentPattern.Content.ToList());
			patterns.Add(newPattern);
		}
		else
		{
			currentPattern.Content[currentLength] = MatchWays.NotExist;

			while (currentPattern.Content[currentLength] <= MatchWays.ExistAtTheSamePosition)
			{
				GenerateAllPatterns(ref patterns, length, currentLength + 1, ref currentPattern);
				currentPattern.Content[currentLength]++;
			}
		}
	}

	public bool IsGuessed()
	{
		return _content.All(matchWay => matchWay == MatchWays.ExistAtTheSamePosition);
	}
}

