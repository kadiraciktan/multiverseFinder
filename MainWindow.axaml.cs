using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace multiverseFinder;

public partial class MainWindow : Window
{
 
    GraphQLHttpClient graphqlClient;


    public MainWindow()
    {

        InitializeComponent();
        graphqlClient = new GraphQLHttpClient("https://rickandmortyapi.com/graphql", new NewtonsoftJsonSerializer());
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(string.IsNullOrEmpty(characterName.Text))
        {
            greetingOutput.Text = "Please enter a charactername!";
            return;
        }
        _ = GetCharacter(characterName.Text!);
        greetingOutput.Text = "Hello, "+characterName.Text+"! Your Multiverse Character List:";

    }

    private async Task GetCharacter(string name)
    {

        var request = new GraphQLRequest
        {
            Query = @"
            query ($name: String!) {
                characters(page: 2, filter: { name: $name }) {
                    info {
                        count
                    }
                    results {
                        name
                    }
                }
            }",
            Variables = new { name }
        };

        var response = await graphqlClient.SendQueryAsync<dynamic>(request);
        string output = "";

        if(response.Errors != null)
        {
            resultBox.Text = "Character not found!";
        }

        if(response.Data.characters.results.Count == 0)
        {
            resultBox.Text = "Nothing... :(";
            return;
        }

        foreach (var character in response.Data.characters.results)
        {
            output += character.name + "\n";
        }

        resultBox.Text = output;
    }

}


 
