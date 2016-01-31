$("a[rel=popover]")
    .popover({
        //offset: 100,
        html: true
    })
    .click(function (e) {
		e.preventDefault();
	});