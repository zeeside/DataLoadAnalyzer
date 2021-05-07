
# DataLoadAnalyzer
Configurable .Net Core utility to migrate data across data-stores, and analyze load times in the destination data-store, publishing a report as output. 

**Sample Configuration for Analyzers**

    "Config": {
    "Analyzers": [
      {
        "Name": "CatalogDataAnalysis",
        "ServiceClass": "CatalogDataAnalyzers",
        "SkipMigration": "true",
        "SkipMigrationIfDataExists": "true",
        "Source": {
          "Type": "Csv",
          "ConfigDictionary": [
            {
              "key": "filepath",
              "value": "C:\\Reports\\data.csv"
            }
          ]
        },
        "Destination": {
          "Type": "ElasticSearch",
          "ConfigDictionary": [
            {
              "key": "index",
              "value": "catalog_data"
            },
            {
              "key": "url",
              "value": "http://es01:9200"
            },
            {
              "key": "PublishBatchSize",
              "value":  10000
            }
          ]
        },
        "QueryDefinitions": [
          {
            "Name": "Load Completed Order Products",
            "Description": "Load 2,000,000 Records",
            "ClassName": "DataLoadAnalyzer.Data.QueryDefinitions.ElasticSearch.CatalogData",
            "Iterations": 20,
            "OutputConfig": {
              "Format": "csv",
              "FilenameFormat": "catalog_data_ec_{0}.csv",
              "Overwrite": "false",
              "Summarize": "true",
              "OutputToConsole":  "true",
              "FilePath":  "c:\\Reports\\CatalogData"
            }
          }
        ]
      }
    ]
