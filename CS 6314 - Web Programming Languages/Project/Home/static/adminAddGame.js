$(document).ready(function()
{
    getGenres();

    $("#uploadImagesButton").click(function(e)
    {
        e.preventDefault();
        let images = document.getElementById("images");
        let imagesList = "";
        
        for(let i=0; i<images.files.length; i=i+1)
        {
            let name = "../static/images/"+images.files[i].name;
            imagesList = imagesList + name + ",";
        }
        imagesList = imagesList.substring(0, imagesList.length-1);
        console.log(imagesList);
        $("#imagesListHidden").val(imagesList);
    });

    $("#uploadCoverImageButton").click(function(e)
    {
        e.preventDefault();
        let image = document.getElementById("coverImage");
        let name = "../static/images/"+image.files[0].name;
        $("#coverImageListHidden").val(name);
    });

    $("#gameSubmitButton").click(function(e)
    {
        e.preventDefault();
        let genre = $("#genre").val();
        console.log(genre);
        let genreList = "";
        for(let i=0; i<genre.length; i=i+1)
            genreList = genreList + genre[i] +",";
        
        genreList = genreList.substring(0, genreList.length-1);
        $("#genreHidden").val(genreList);

        $.ajax(
        {
            url: "testSubmit",
            type: "POST",
            data: $("#gameForm").serialize(),
            success: function(response)
            {
                let responseJSON = JSON.parse(response);
                if(responseJSON.message == "Successful")
                    window.location.href = "/adminIndex";
            },
            error: function(error)
            {
                console.log(error);
            }
        });
    });
});

function getGenres()
{
    $.ajax(
    {
        url: "/getGenres",
        success: function(response)
        {
            let responseJSON = JSON.parse(response);
            if(responseJSON.length > 0)
            {
                for(let i=0; i<responseJSON.length; i=i+1)
                    $("#genre").append("<option value=\""+responseJSON[i]+"\">"+responseJSON[i]+"</option>");   
            }
        },
        error: function(error)
        {
            console.log(error);
        }
    });
}