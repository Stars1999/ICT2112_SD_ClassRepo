namespace ICT2106WebApp.mod1grp4
{
	// iTableLatexConversion (Andrea - COMPLETED)
	public interface iTableLatexConversion
	{
		public Task<List<Table>> convertToLatexAsync(List<Table> tableList); // Asynchronous cell conversion
	}
}
