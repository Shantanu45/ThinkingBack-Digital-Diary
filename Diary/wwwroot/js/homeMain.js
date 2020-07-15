function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}
$(".colorpad").toArray().forEach(element => {
    element.style.backgroundColor =  '#' + Math.floor(Math.random()*16777215).toString(16);
});

$( ".card" ).hover(
    function() {
      $(this).addClass('shadow border-primary').css('cursor', 'pointer'); 
    }, function() {
      $(this).removeClass('shadow border-primary');
    }
  );

  $( ".navigation-icons" ).hover(
    function() {
      $(this).addClass('shadow-sm border-primary').css('cursor', 'pointer'); 
    }, function() {
      $(this).removeClass('shadow-sm border-primary');
    }
  );
