using System.Text.RegularExpressions;

namespace RegexNonBacktracking;

public static partial class QueryStringObfuscatorRegex
{
    public const RegexOptions DefaultOptions = RegexOptions.Compiled |
                                               RegexOptions.IgnoreCase |
                                               RegexOptions.IgnorePatternWhitespace |
                                               RegexOptions.ExplicitCapture;

    public const string DefaultRegexPattern =
        """
        (?ix)
         (?: # JSON-ish leading quote
            (?:"|%22)?
         )
         (?: # common keys
            (?:old[-_]?|new[-_]?)?p(?:ass)?w(?:or)?d(?:1|2)? # pw, password variants
           |pass(?:[-_]?phrase)?  # pass, passphrase variants
           |secret
           |(?: # key, key_id variants
                api[-_]?
               |private[-_]?
               |public[-_]?
               |access[-_]?
               |secret[-_]?
               |app(?:lication)?[-_]?
            )key(?:[-_]?id)?
           |token
           |consumer[-_]?(?:id|key|secret)
           |sign(?:ed|ature)?
           |auth(?:entication|orization)?
         )
         (?:
            # '=' query string separator, plus value til next '&' separator
            (?:\s|%20)*(?:=|%3D)[^&]+
            # JSON-ish '": "somevalue"', key being handled with case above, without the opening '"'
           |(?:"|%22)                                     # closing '"' at end of key
            (?:\s|%20)*(?::|%3A)(?:\s|%20)*               # ':' key-value separator, with surrounding spaces
            (?:"|%22)                                     # opening '"' at start of value
            (?:%2[^2]|%[^2]|[^"%])+                       # value
            (?:"|%22)                                     # closing '"' at end of value
         )
        |(?: # other common secret values
            bearer(?:\s|%20)+[a-z0-9._\-]+
           |token(?::|%3A)[a-z0-9]{13}
           |gh[opsu]_[0-9a-zA-Z]{36}
           |ey[I-L](?:[\w=-]|%3D)+\.ey[I-L](?:[\w=-]|%3D)+(?:\.(?:[\w.+/=-]|%3D|%2F|%2B)+)?
           |-{5}BEGIN(?:[a-z\s]|%20)+PRIVATE(?:\s|%20)KEY-{5}[^\-]+-{5}END(?:[a-z\s]|%20)+PRIVATE(?:\s|%20)KEY(?:-{5})?(?:\n|%0A)?
           |(?:ssh-(?:rsa|dss)|ecdsa-[a-z0-9]+-[a-z0-9]+)(?:\s|%20|%09)+(?:[a-z0-9/.+]|%2F|%5C|%2B){100,}(?:=|%3D)*(?:(?:\s|%20|%09)+[a-z0-9._-]+)?
         )
        """;

    public const string ExperimentalRegexPattern =
        """
        (?ix)
         (?:"|%22)?
         (?: # common keys
             # pw, password, pass, passphrase variants
            (?:(?:old|new)[-_]?)?p(?:(?:(?:ass)?w(?:or)?d(?:1|2)?)|(?:ass(?:[-_]?phrase)?)) 
           |s(ecret|ign(?:ed|ature)?)
           |(?: # key, key_id variants
                a(pi|ccess|pp(?:lication)?)
               |p(rivate|ublic)
               |secret
            )[-_]?key(?:[-_]?id)?
           |token
           |consumer[-_]?(?:id|key|secret)
           |auth(?:entication|orization)?
         )
         (?:
            # '=' query string separator, plus value til next '&' separator
            (?:\s|%20)*(?:=|%3D)(?:[^&])+
            # JSON-ish '": "somevalue"', key being handled with case above, without the opening '"'
           |(?:"|%22)                                     # closing '"' at end of key
            (?:\s|%20)*(?::|%3A)(?:\s|%20)*               # ':' key-value separator, with surrounding spaces
            (?:"|%22)                                     # opening '"' at start of value
            (?:%2[^2]|%[^2]|[^"%])+                       # value
            (?:"|%22)                                     # closing '"' at end of value
         )
        |(?: # other common secret values
            bearer(?:\s|%20)+(?:[a-z0-9._\-])+
           |token(?::|%3A)(?:[a-z0-9]){13}
           |gh[opsu]_(?:[0-9a-zA-Z]){36}
           |ey[I-L](?:[\w=-]|%3D)+\.ey[I-L](?:[\w=-]|%3D)+(?:\.(?:[\w.+/=-]|%3D|%2F|%2B)+)?
           |(?:-){5}BEGIN(?:[a-z\s]|%20)+PRIVATE(?:\s|%20)KEY-{5}(?:[^\-])+-{5}END(?:[a-z\s]|%20)+PRIVATE(?:\s|%20)KEY(?:-{5})?(?:\n|%0A)?
           |(?:ssh-(?:rsa|dss)|ecdsa-(?:[a-z0-9])+-(?:[a-z0-9])+)(?:\s|%20|%09)+(?:[a-z0-9/.+]|%2F|%5C|%2B){100,}(?:=|%3D)*(?:(?:\s|%20|%09)+(?:[a-z0-9._-])+)?
         )
        """;
}
