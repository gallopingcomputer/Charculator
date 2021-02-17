using System;
using System.Collections.Generic;
using System.Linq;

//TODO: note that this is actually more of a "ByteRadix" class than a plain "Radix" - PrintByteSequence already introduces some notion of "grouping" (by byte), but this one is special since it might not happen on digit boundaries

public class Radix
{
    public static implicit operator byte(Radix x) => x.Value;
    public byte Value { get; private set; }
    public bool IsReducible { get; private set; }
    public byte DigitsPerByte { get; private set; }
    public Radix(byte value) {
        Value = value;
        (DigitsPerByte, IsReducible) = GetDigitsPerByte();
    }

    protected (byte, bool) GetDigitsPerByte()
    =>  GetDigitCountForNumber(256);

    // width is the number of bytes (also it can't be too large)
    // zero in, zero out
    public (byte, bool) GetDigitCount(byte width = 1)
    =>  GetDigitCountForNumber((uint) MathUtils.Power(2, (byte)(width * 8)));

    // Returns the minimum number of digits to represent the range [0, number) in the specified base, i.e. the ceiling of the logarithm. The boolean field in the result specifies whether number is an integer power of radix, i.e. whether it lies just outside the range representable by (result) digits.
    public (byte, bool) GetDigitCountForNumber(ulong number)
    {
        byte i = 0;
        uint ac = 1;
        
        while (ac < number) { ac *= Value; i++; }
        return (i, (ac == number));
    }
}

public class RadixWithGrouping : Radix
{
    public 
    enum GroupingModeType
    { Neutral = -1, None = 0, Forced }
    // "Forced": in the integer-type GroupingMode parameters, any positive number will be interpreted as the number of bytes in a grouped sequence
    // "Neutral": conversions taking List<byte[]>, which are naturally delimited byte sequences, can use this option.

    public static GroupingModeType GetGroupingMode(int grouping_mode)
    {
        if (grouping_mode < 0) return GroupingModeType::Neutral;
        else if (grouping_mode == 0) return GroupingModeType::None;
        else return GroupingModeType::Forced;
    }

    public byte GroupWidth { get; private set; }
    public GroupingModeType GroupingMode {
        get => GetGroupingMode(GroupWidth);
    }
    public bool IsGroupReducible { get; private set; }
    public byte DigitsPerGroup { get; private set; }

    public RadixWithGrouping(byte rd, byte gr = 0) : base(rd)
    {
        GroupWidth = gr;
        if (base.IsReducible)
            (DigitsPerGroup, IsGroupReducible) = ((byte)(DigitsPerByte * gr), true);
            //!!! - DigitsPerByte * gr is limited to 255, which seems reasonable; I suggest artificially limiting it to 64. (Move this comment to become part of the function description later on.)
        else
            (DigitsPerGroup, IsGroupReducible) = base.GetDigitCount(GroupWidth);
    }
}


public class RadixPrinter
{
    public BindingTarget<RadixWithGrouping> rg;
    public BindingTarget<string> byte_separator;
    // public BindingTarget<Radix> radix;
    // public BindingTarget<byte> grouping_mode;
    public BindingTarget<string> word_separator;

    public Radix radix { get => rg.Val; }
    public byte grouping_mode { get => rg.Val.GroupWidth; }

    public BindingSource<string> result;

    public RadixPrinter(
        RadixWithGrouping rg, 
        string byte_separator, string word_separator
    ) : rg(Materialize),
        byte_separator(Materialize),
        word_separator(Materialize)
    {
        if (byte_separator != "" && word_separator == "")
            throw new ArgumentOutOfRangeException();
        //TODO: more validation

        this.rg.Val = rg;
        this.byte_separator.Val = byte_separator;
        this.word_separator.Val = word_separator;

        //TODO: chain 

        //TODO: need to mark all three as "clean" when doing update
        //TODO: that's the main problem - how do you do that without having to trigger three separate update events??? => will have to override setValue
        //TODO: here's a better idea - maybe we should use the tuple thing and then expose the elements separately; but how to make the binding work???? => bind separately but refresh jointly? => might need to break up CachedUpdate; the resulting "Source" type will be similar (but not identical) to "Result"
        //TODO: here's a even better idea - maybe we should expose different setters for each 
    }

    public override 
    String Convert(byte[] input)
    {
        return rg.PrintFormattedByteSequence(input, byte_separator, rg.GroupWidth, word_separator);
    }
}

public class RadixPrinterEx : BoundConverter<List<byte[]>, String>
{
    public string byte_separator { get; private set; }
    public Radix radix { get; private set; }
    public byte grouping_mode { get; private set; }
    public string word_separator { get; private set; }
    public string error_string { get; private set; }

    public 
    RadixPrinterEx(byte radix, int grouping_mode, 
        string byte_separator, string word_separator, string error_string)
    {
        //TODO: need to verify that settings are valid and error string isn't ambiguous!!!

        this.radix = new Radix(radix);
        this.grouping_mode = grouping_mode;
        this.byte_separator = byte_separator;
        this.word_separator = word_separator;
        this.error_string = error_string;
    }

    public override 
    String Convert(List<byte[]> input)
    {
        return radix.PrintFormattedByteSequenceEx(input, grouping_mode, error_string, word_separator, byte_separator);
    }
}

