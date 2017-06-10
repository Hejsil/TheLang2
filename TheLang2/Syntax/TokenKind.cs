namespace TheLang2.Syntax
{
    public enum TokenKind
    {
        // Unary operators
        ExclamationMark,
        At,

        // Binary operators
        Plus,
        Minus,
        Times,
        Divide,
        EqualEqual,
        Modulo,
        LessThan,
        LessThanEqual,
        GreaterThan,
        GreaterThanEqual,
        ExclamationMarkEqual,
        And,
        Or,
        Dot,

        Equal,
        PlusEqual,
        MinusEqual,
        TimesEqual,
        DivideEqual,
        ModulusEqual,
        
        DotDot,

        Identifier,
        Directive,
        FloatNumber,
        DecimalNumber,
        String,

        KeywordStruct,
        KeywordTrue,
        KeywordFalse,
        KeywordReturn,
        KeywordNew,
        KeywordDelete,
        KeywordDefer,
        KeywordIf,
        KeywordElse,
        KeywordCase,
        KeywordLoop,
        KeywordBreak,
        KeywordContinue,

        Arrow,

        SquareLeft,
        SquareRight,

        ParenthesesLeft,
        ParenthesesRight,

        CurlyLeft,
        CurlyRight,

        Colon,
        SemiColon,
        Comma,

        EndOfFile,
        Unknown
    }
}
