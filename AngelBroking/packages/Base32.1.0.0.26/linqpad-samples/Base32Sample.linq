<Query Kind="Statements">
  <NuGetReference>Base32</NuGetReference>
  <Namespace>Base32</Namespace>
</Query>

var data = new byte[] { 1,2,3 };
data.Dump("Initial Data");
var encodedData = Base32Encoder.Encode(data).Dump("Encoded Data");
Base32Encoder.Decode(encodedData).Dump("Decoded Data");