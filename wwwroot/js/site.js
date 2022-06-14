
$(() => {
    LoadProdData();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalServer")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    

    connection.on('SetUsersOnline', usersOnline => {
        if (usersOnline.length > 0) {
            $('#onlineUsers').innerText = '';
            $.each(usersOnline, function (i, user) {
                addUserOnline(user);
            });
        }
    });

    connection.start().catch(err => console.error(err.toString()));

    // ончардж фильтрации (фильтрация при изменении значений полей) 
    $('#myFactory').change(function () {
        LoadProdData();
    })
    LoadProdData();

    $('#myCarrier').change(function () {
        LoadProdData();
    })
    LoadProdData();

    $('#myQueue').change(function () {
        LoadProdData();
    })
    LoadProdData();

    $('#Search').keyup (function () {
        LoadProdData();
    })
    LoadProdData();

    connection.on("LoadQueueItems", function () {
        LoadProdData();
    })
    LoadProdData();



    $("#myFactory").on('change', function () {
       // $('#myQQQ').prepend('<option value="">новый option</option>');
      //  $("#myQQQ").empty();
       // $('#myQQQ').prepend('<option value="">новый option</option>');
        LoadQueuesList();
       // $("#myQQQ").empty();

    });
    
    function LoadQueuesList(value) {

        var jsonData = JSON.stringify({
        aData: value
        });

        $.ajax({
            url: '/Queues/GetQueue',
            data: 'Search=' + ($(".form-control").val()) + '&FactoryName=' + ($("#myFactory option:selected").text()),
            method: 'GET',
            success: (viewModel) => {
                /*    $.each(viewModel.QueueItems, function (key,data) {
                         tr += `<tr>
                           <td>${data.OderLine}</td>
                           <td>${data.CarNumber}</td>
                           <td>${data.Barecode}</td>
                           <td>${data.Brand}</td>
                           <td>${data.Package}</td>
                           <td>${data.DriverName}</td>
                           <td>${data.FactoryId}</td>
     
                           <td>
                           <a href ='../QueueItems/Edit?id=${data.QueueID}'>Edit</a>
                           <a href ='../QueueItems/Details?id=${data.QueueID}'>Details</a>
                           <a href ='../QueueItems/Delete?id=${data.QueueID}'>Delete</a>
                           </td>   QueueName = "ZZZHGG № 3"
                      </tr>`;
                     })*/
                // $('#QQQ').prepend('<option value="">новый option</option>');
                var frag;
                $("#myQueue").empty();
                $.each(viewModel.Queue, function (key, data) {
                    $('#myQueue').prepend('<option value="' + key + '">' + data.QueueName + '</option>');

                }) 
            },
            error: (error) => {
                console.log(error)
            }


        });


       /* function OnSuccess(response) {
            var aData = response.d
            $("#myQQQ").empty();
            var frag;
            $.each(aData, function (key, data) {
                frag += "<li>" + data.OderLine + "</li>";
            });

            $("#myQQQ").append(frag);
        }*/
        function OnErrorCall(response) { }
    }


    function LoadProdData(id) {
        var tr = '';

        $.ajax({
            url: '/QueueItems/GetQueueItems',
            data: 'Search=' + ($(".form-control-lg").val()) + '&FactoryName=' + ($("#myFactory option:selected").text()) + '&CarrierName=' +
                ($("#myCarrier option:selected").text()) + '&QueueName=' + ($("#myQueue option:selected").text()) ,
            method: 'GET',
            success: (viewModel) => {
                /*    $.each(viewModel.QueueItems, function (key,data) {
                         tr += `<tr>
                           <td>${data.OderLine}</td>
                           <td>${data.CarNumber}</td>
                           <td>${data.Barecode}</td>
                           <td>${data.Brand}</td>
                           <td>${data.Package}</td>
                           <td>${data.DriverName}</td>
                           <td>${data.FactoryId}</td>
     
                           <td>
                           <a href ='../QueueItems/Edit?id=${data.QueueID}'>Edit</a>
                           <a href ='../QueueItems/Details?id=${data.QueueID}'>Details</a>
                           <a href ='../QueueItems/Delete?id=${data.QueueID}'>Delete</a>
                           </td>
                      </tr>`;
                     })*/
               // $('#QQQ').prepend('<option value="">новый option</option>');
                $.each(viewModel.QueueItems, function (key, data) {
                    tr += `<tr>
                           <td>${data.OderLine}</td>
                           <td>${data.CarNumber}</td>
                           <td>${data.Barecode}</td>
                           <td>${data.QueueName}</td>
                           <td>${data.DriverName}</td >
                           <td>${data.CarrierName}</td>
                           <td>${data.FactoryName}</td>
                           <td>${data.CreatedAt.substring(0, 10)}${" "+data.CreatedAt.substring(11, 19)}</td >
                           <td>
                           <a class=hide href ='../QueueItems/Edit?QueueItemId=${data.QueueItemId}'>Edit</a>
                           <a class=hide href='../QueueItems/Details?QueueItemId=${data.QueueItemId}'><h5>Подробнее</h5></a>
                           <a class=hide href ='../QueueItems/Delete?QueueItemId=${data.QueueItemId}'>Delete</a>
                           </td>
                      </tr>`;
                   

                })
                $.each(viewModel.QueueItems.Factory, function (key, data) {
                        // console.log([['Barecode'] + key + data]);
                        tr += `<tr>
                         <td>${data.OrganizationName}</td>
                       </tr>`;
                    })
                $("#tableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }


        });

    }



})


