using gRPCClient.DatabaseOperations;

VagtOperations vagtOperations = new VagtOperations("http://localhost:9090");

//vagtOperations.FjernVagt("3");

// vagtOperations.OpretVagt("19-11-2022", "18", "30", "23", "30", "1", "30", "1");

vagtOperations.OpretMedarbejder("Mads","Bob","mail@mail.com","12345678","vejnavn1");

//vagtOperations.RedigerVagt("5", "19-11-2022", "20", "45", "23", "45", "2", "15", "1");