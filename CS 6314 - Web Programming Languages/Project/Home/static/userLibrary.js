var data;
$(document).ready(function()
{
    $.ajax(
        {
            url: "/userLibrary",
            method: "GET",
            success: function(response)
            {
                var index = 0;
                // console.log(response);
                response = JSON.parse(response);
                response.forEach(i =>
                {
                    console.log(i);
                });
            },
            error: function(e)
            {
                console.log("Error: "+e);
            }
        }
    );

    var librarySearchTerm = document.getElementById("librarySearchField").value;
    $.ajax(
    {
        url: "/userLibrary",
        type: "POST",
        data: librarySearchTerm,
        success: function(response)
        {
            data = JSON.parse(response);
            displayLibraryResults();
        },
        error: function(error)
        {
            console.log(error);
        }
    });

    var searchTerm = document.getElementById("searchField").value;
    $.ajax(
    {
        url: "/search",
        type: "POST",
        data: searchTerm,
        success: function(response)
        {
            data = JSON.parse(response)
            displayResults();
        },
        error: function(error)
        {
            console.log(error);
        }
    });

    $("#searchResults").on("click", "a", function(e)
    {
        let gameId = $(this).siblings(".gameId").val();
        $(this).attr("href", "/showDetails?gameId="+gameId);
    });

    $("#librarySearchResults").on("click", "a", function(e)
    {
        let gameId = $(this).siblings(".gameId").val();
        $(this).attr("href", "/showDetails?gameId="+gameId);
    });
});

function displayResults()
{
    $("#searchTermJumbotron").text($("#searchField").val());
    if(data.length == 0)
    {
        let display = "<div class=\"jumbotron jumbotron-fluid\" id=\"nothingFound\">"+
                            "<div class=\"container\">"+
                                "<h1 class=\"display-4 text-center\">No games found...</h1>"+
                                "<p class=\"lead text-center\">Try using a different keyword.</p>"+
                            "</div>"+
                        "</div>";
        $("#nothingFound").remove();
        $("body").append(display);
    }

    else
    {
        displayFilters();
        for(let i=0; i<data.length; i=i+1)
        {
            let card = "<div class=\"card\" style=\"width: 18rem; margin: 2%\">"+
                            "<img class=\"card-img-top\" src=\""+data[i][6]+"\" alt=\""+data[i][7]+"\">"+
                            "<div class=\"card-body\">"+
                                "<h5 class=\"card-title\">"+data[i][1]+"</h5>"+
                                "<p class=\"card-text\">"+data[i][2].substring(0, 70)+"...</p>"+
                            "</div>"+
                            "<div class=\"card-footer text-center\">"+
                                "<input type=\"hidden\" class=\"gameId\" value=\""+data[i][0]+"\">"+
                                "<a href=\"#\" class=\"btn btn-link\">See details</a>"+
                            "</div>"+
                        "</div>";

            $("#searchResults").append(card);
        }
    }
}

function displayLibraryResults()
{
    $("#searchTermJumbotron").text($("#librarySearchField").val());
    if(data.length == 0)
    {
        let display = "<div class=\"jumbotron jumbotron-fluid\" id=\"nothingFound\">"+
                            "<div class=\"container\">"+
                                "<h1 class=\"display-4 text-center\">No games found...</h1>"+
                                "<p class=\"lead text-center\">Try using a different keyword.</p>"+
                            "</div>"+
                        "</div>";
        $("#nothingFound").remove();
        $("body").append(display);
    }

    else
    {
        // displayFilters();
        for(let i=0; i<data.length; i=i+1)
        {
            let card = "<div class=\"card\" style=\"width: 18rem; margin: 2%\">"+
                            "<img class=\"card-img-top\" src=\""+data[i][6]+"\" alt=\""+data[i][7]+"\">"+
                            "<div class=\"card-body\">"+
                                "<h5 class=\"card-title\">"+data[i][1]+"</h5>"+
                                "<p class=\"card-text\">"+data[i][2].substring(0, 70)+"...</p>"+
                            "</div>"+
                            "<div class=\"card-footer text-center\">"+
                                "<input type=\"hidden\" class=\"gameId\" value=\""+data[i][0]+"\">"+
                                "<a href=\"#\" class=\"btn btn-link\">See details</a>"+
                            "</div>"+
                        "</div>";

            $("#searchResults").append(card);
        }
    }
}
