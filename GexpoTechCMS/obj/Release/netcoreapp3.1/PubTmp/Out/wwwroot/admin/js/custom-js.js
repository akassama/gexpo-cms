// document ready function
$(document).ready(function () {
	//activate tooltip
	var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
		var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
			  return new bootstrap.Tooltip(tooltipTriggerEl)
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//checks if data already exists for model
	//on keyup of callback validation input
	$(".callback-input").keyup(function () {
		var input_value = this.value;
		const input_type = $(this).data("callback-type");
		var val = input_value + "[#]" + input_type;

		$.getJSON("/Api/VerifyUniqueData", { key: val }, function (response) {
			//if response contains data
			if (containsString(response, "success") && containsString(response, "#")) {
				var data = response.split('#');
				var data_val = isEmpty(data[1]) ? "Data" : data[1];
				var split_dat_val = data_val.match(/[A-Z][a-z]+|[0-9]+/g).join(" ");
				var validation_msg = split_dat_val + " already exists";
				var callback_validation_class = "callback-input-validation-" + data_val.toLowerCase();
				$("." + callback_validation_class).show();
				$("." + callback_validation_class).html(validation_msg);
				$('.callback-validation-btn').prop('disabled', true);
			} 
			else {
				$('[class*="callback-input-validation"]').hide();
				$('.callback-validation-btn').prop('disabled', false);
			}
		});
	});



	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//open file select on profile image click
    $('.upload-file-button').on('click', function (e) {
        $('.upload-file-select').trigger('click');
    });

    //set profile image on profile image click
    $('.upload-file-select').change(function (e) {
        var fileName = e.target.files[0].name;
        $(".file-preview-name").text(fileName);

        var reader = new FileReader();
        reader.onload = function (e) {
            // get loaded data and render thumbnail.
            $(".file-preview").attr('src', e.target.result);
        };
        // read the image file as a data URL.
        reader.readAsDataURL(this.files[0]);

        //show/hide image preview div if existing
        if(fileName !== ""){
        	$('.file-preview-div').addClass('d-block').removeClass('d-none');
        }	
        else{
        	$('.file-preview-div').addClass('d-none').removeClass('d-block');
        }
    });


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//Checks for character and sets info for input
	$(".count-chars").keyup(function () {
		//get input value and length
		var charInput = this.value;
		var charInputLength = this.value.length;
		
		//get data values
		const maxChars = $(this).data("chars-max");
		const messageColor = $(this).data("msg-color");
		
		//get input id and set input message id
		var inputId = this.getAttribute('id');			
		var messageDivId = inputId+"Message";
		
		//set default message for message div
		var remainingMessage = "";
		
		if (charInputLength >= maxChars) {
			//limit chars to max set
			$("#"+inputId).val(charInput.substring(0, maxChars));
			remainingMessage = "0 characters remaining";
		} else {
			remainingMessage = (maxChars - charInputLength) + " character(s) remaining";
		}
		
		
		//check if message div exists
		if($("#" + messageDivId).length == 0) {
		  //set div with message
		  $('#'+inputId).after('<div id="'+messageDivId+'" class="text-'+messageColor+' font-weight-bold">'+remainingMessage+'</div>');
		}
		else{
		  //update div message 
		  $("#" + messageDivId).text(remainingMessage);
		}
	});


	//Checks for character remaining and displays when 0
	$(".count-reach").keyup(function () {
		//get input value and length
		var charInput = this.value;
		var charInputLength = this.value.length;
		
		//get data values
		const maxChars = $(this).data("chars-max");
		const messageColor = $(this).data("msg-color");
		
		//get input id and set input message id
		var inputId = this.getAttribute('id');			
		var messageDivId = inputId+"Message";
		
		//set default message for message div
		var remainingMessage = "";
		
		if (charInputLength >= maxChars) {
			//limit chars to max set
			$("#"+inputId).val(charInput.substring(0, maxChars));
			remainingMessage = "0 characters remaining";
		} 
		
		//check if message div exists
		if($("#" + messageDivId).length == 0) {
		  //set div with message
		  $('#'+inputId).after('<div id="'+messageDivId+'" class="text-'+messageColor+' font-weight-bold">'+remainingMessage+'</div>');
		}
		else{
		  //update div message 
		  $("#" + messageDivId).text(remainingMessage);
		}
	});


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	// BS5 form validation
	(function () {
	  'use strict'

	  // Fetch all the forms we want to apply custom Bootstrap validation styles to
	  var forms = document.querySelectorAll('.needs-validation')

	  // Loop over them and prevent submission
	  Array.prototype.slice.call(forms)
	    .forEach(function (form) {
	      form.addEventListener('submit', function (event) {
	        if (!form.checkValidity()) {
	          event.preventDefault()
	          event.stopPropagation()
	        }

	        form.classList.add('was-validated')
	      }, false)
	    })
	})();

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//Password match validation
	$(".re-typed-password").keyup(function () {
		var password = $(".typed-password").val();
		var confirmPassword = $(".re-typed-password").val();

		if (password == confirmPassword && password.length > 1) {
			//set div with message
			setPasswordMatchMessage("success", "Passwords match");
		}
		else {
			//set div with message
			setPasswordMatchMessage("danger", "Passwords do not match");
		}

		function setPasswordMatchMessage(messageColor, validationMessage){
			//check if message div exists
			if($("#match-validation-display").length == 0) {
			  //set div with message
			  $('.re-typed-password').after('<div id="match-validation-display" class="text-'+messageColor+' font-weight-bold">'+validationMessage+'</div>');
			}
			else{
			  //update div message 
			  $("#match-validation-display").text(validationMessage);
			  
			  //update class
			  var newValidationClass = "text-"+messageColor+" font-weight-bold";
			  $("#match-validation-display").attr('class', newValidationClass);
			}
		}
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//set summernote text editor
	$('.text-editor').summernote({
        placeholder: 'Type in text',
        tabsize: 2,
        height: 100
    });

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//Set SEO Values
	$(".seo-title").keyup(function () {
		var postTitle = this.value;
		$('.seo-title-data').val(postTitle);
		$('.seo-description-data').val(postTitle);
		$(".seo-keywords-data").val(setKeywords(postTitle));

		var hostname = window.location.origin;

		//set SEO preview
		if (postTitle !== "") {
			$('.seo-display-div').addClass('d-block').removeClass('d-none');
			$('.seo-display-title').text(postTitle);
			$('.seo-display-link').text(hostname + "/" + convertToSlug(postTitle));
			var formatDate = getMonthName() + " " + getCurrentDay() + ", " + getYearName();
			$('.seo-display-date').text(formatDate + " - " + postTitle);
		}
		else {
			$(".seo-display-div").css("display", "none");
			$('.seo-display-div').addClass('d-none').removeClass('d-block');
		}

		//generates csv keywords from title
		function setKeywords(title) {
			var exclude_array = ['a', 'about', 'all', 'also', 'and', 'as', 'at', 'be', 'because', 'but', 'by', 'can', 'come', 'could', 'day', 'do', 'even', 'find', 'first', 'for', 'from', 'get', 'give', 'go', 'have', 'he', 'her', 'here', 'him', 'his', 'how', 'I', 'if', 'in', 'into', 'it', 'its', 'just', 'know', 'like', 'look', 'make', 'man', 'many', 'me', 'more', 'my', 'new', 'no', 'not', 'now', 'of', 'on', 'one', 'only', 'or', 'other', 'our', 'out', 'people', 'say', 'see', 'she', 'so', 'some', 'take', 'tell', 'than', 'that', 'the', 'their', 'them', 'then', 'there', 'these', 'they', 'thing', 'think', 'this', 'those', 'time', 'to', 'two', 'up', 'use', 'very', 'want', 'way', 'we', 'well', 'what', 'when', 'which', 'who', 'will', 'with', 'would', 'year', 'you', 'your', 'has', 'was', 'why'];
			var keywords = "";
			var split = title.split(" ");
			for (var i = 0; i < split.length; i++) {
				if (split[i].length > 2 && (jQuery.inArray(split[i].toLowerCase(), exclude_array) < 0)) {
					keywords += split[i] + ",";
				}
			}

			//remove last comma
			keywords = keywords.replace(/,\s*$/, "");
			return keywords;
		}
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//set seo display for edit view
	if ($('.seo-title').val()) {

		var postTitle = $(".seo-title").val();
		var hostname = window.location.origin;

		//set SEO preview
		if (postTitle !== "") {
			$('.seo-display-div').addClass('d-block').removeClass('d-none');
			$('.seo-display-title').text(postTitle);
			$('.seo-display-link').text(hostname + "/" + convertToSlug(postTitle));
			var formatDate = getMonthName() + " " + getCurrentDay() + ", " + getYearName();
			$('.seo-display-date').text(formatDate + " - " + postTitle);
		}
		else {
			$(".seo-display-div").css("display", "none");
			$('.seo-display-div').addClass('d-none').removeClass('d-block');
		}
	}

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//if viewing edit post, toggle gallery images if gallery images exist in edt
	//checks if the class exists
	if ($(".current-gallery-image")[0]) {
		// toggle gallery image sview
		$('a.toggle-gallery-images')[0].click();
	} 

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//text to slug function
	function convertToSlug(Text)
	{
	    return Text
	        .toLowerCase()
	        .replace(/[^\w ]+/g,'')
	        .replace(/ +/g,'-')
	        ;
	}
	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/

	//Get current date
	function getCurrentDate(){
		var today = new Date();
		var dd = today.getDate();

		var mm = today.getMonth()+1; 
		var yyyy = today.getFullYear();
		if(dd<10) 
		{
			dd='0'+dd;
		} 

		if(mm<10) 
		{
			mm='0'+mm;
		}
	  
	  today = dd+'-'+mm+'-'+yyyy;
	  return today;
	}

	//Get current date month
	function getMonthName() {
	  var month = new Array();
	  month[0] = "January";
	  month[1] = "February";
	  month[2] = "March";
	  month[3] = "April";
	  month[4] = "May";
	  month[5] = "June";
	  month[6] = "July";
	  month[7] = "August";
	  month[8] = "September";
	  month[9] = "October";
	  month[10] = "November";
	  month[11] = "December";

	  var d = new Date();
	  var month = month[d.getMonth()];

	  return month;
	}

	//Get current date year
	function getYearName(){
		var d = new Date();
  		var year = d.getFullYear();
  		return year;
	}

	//Get current day
	function getCurrentDay(){
		var d = new Date();
  		var day = d.getDate();
  		return day;
	}

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	

	//Image gallery preview
	$(".imgAdd").click(function(){
	  $(this).closest(".row").find('.imgAdd').before('<div class="col-sm-3 imgUp"><div class="imagePreview"></div><label class="btn btn-primary">Upload<input type="file" name="ImageGallery" id="ImageGallery" class="uploadFile img" accept="image/*" value="Upload Photo"></label><img src="https://png.pngtree.com/png-vector/20190603/ourmid/pngtree-icon-close-button-png-image_1357955.jpg" class="del" alt="" width="24" height="24"> </div>');
	});
	$(document).on("click", "img.del" , function() {
		$(this).parent().remove();
	});
	$(function() {
	    $(document).on("change",".uploadFile", function()
	    {
	    	var uploadFile = $(this);
	        var files = !!this.files ? this.files : [];
	        if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support
	 
	        if (/^image/.test( files[0].type)){ // only image file
	            var reader = new FileReader(); // instance of the FileReader
	            reader.readAsDataURL(files[0]); // read the local file
	 
	            reader.onloadend = function(){ // set image data as background of div
					uploadFile.closest(".imgUp").find('.imagePreview').css("background-image", "url("+this.result+")");
	            }
	        }
	      
	    });
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	

	//edit input click
	$('.edit-record').on('click', function (e) {

		//get clicked element key/id
		var modalTitle = $(this).data("title");
		var modalLabelName = $(this).data('label');
		var recordKey = $(this).data('key');
		var inputValue = $(this).data('value');
		var inputLength = $(this).data('max-length');
		var validateInput = $(this).data('validate');
		var editModel = $(this).data('model');
		var returnView = $(this).data('view');
		var returnViewID = $(this).data('route-id');
		var formAction = $(this).data('action');

		//check if main inputs are empty
		if (isEmpty(recordKey) || isEmpty(inputValue) || isEmpty(editModel) || isEmpty(formAction)) {
			alert("missng required data values");
			return false;
		}

		//set modal title
		$(".edit-modal-title").text(modalTitle);

		//set modal input label
		$(".edit-modal-label").text(modalLabelName);

		//set input value
		$(".edit-modal-input").val(inputValue);

		//set hidden input edit model value
		$(".edit-modal-model").val(editModel);

		//set hidden input return view value
		$(".edit-modal-view").val(returnView);

		//set hidden input return view id value
		$(".edit-modal-route-id").val(returnViewID);

		//set max input value
		if (isEmpty(inputLength)) {
			inputLength = "500";
		}
		$('.edit-modal-input').attr('data-chars-max', inputLength);
		$(".edit-modal-input").attr('maxlength', inputLength);

		//set hidden input key value
		$(".edit-modal-key").val(recordKey);

		//set on validate message
		if (!isEmpty(validateInput)) {
			$(".edit-modal-validation").text("Please provide input for " + validateInput);
		}
		else {
			$(".edit-modal-validation").text("Input field required.");
		}

		//set form action
		$('.edit-modal-action').attr('action', formAction);

		//show modal
		$('.edit-modal').modal('show');

	});


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	

	//delete record click
	$('.delete-record').on('click', function (e) {
		//get clicked element key/id
		var modalTitle = $(this).data("title");
		var modalLabelName = $(this).data('label');
		var recordKey = $(this).data('key');
		var deleteModel = $(this).data('model');
		var returnView = $(this).data('view');
		var returnViewID = $(this).data('route-id');
		var formAction = $(this).data('action');

		//check if main inputs are empty
		if (isEmpty(modalLabelName) || isEmpty(recordKey) || isEmpty(formAction)) {
			alert("missng required data values");
			return false;
		}

		//set modal title
		$(".delete-modal-title").text(modalTitle);

		//set modal input label
		$(".delete-modal-label").text(modalLabelName);

		//set hidden input key value
		$(".delete-modal-key").val(recordKey);

		//set hidden input delete model value
		$(".delete-modal-model").val(deleteModel);

		//set hidden input return view value
		$(".delete-modal-view").val(returnView);

		//set hidden input return view id value
		$(".delete-modal-route-id").val(returnViewID);

		//set form action
		$('.delete-modal-action').attr('action', formAction);

		//show modal
		$('.delete-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	$('.edit-category').on('click', function (e) {
		//get clicked element key/id
		var categoryID = $(this).data("id");
		var categoryName = $(this).data('name');
		var categoryDescription = $(this).data('description');
		var categoryIcon = $(this).data('icon');
		var categoryStatus = $(this).data('status');
		var categoryOrder = $(this).data('order');

		//check if main inputs are empty
		if (isEmpty(categoryID) || isEmpty(categoryName) || isEmpty(categoryOrder) || isEmpty(categoryStatus)) {
			alert("missng required data values");
			return false;
		}

		//set modal inputs
		$(".category-id").val(categoryID);
		$(".category-name").val(categoryName);
		$(".short-Description").val(categoryDescription);
		$(".category-icon").val(categoryIcon);

		//set modal select options
		if ($.trim(categoryStatus) == '1') {
			$('#CategoryStatus').find(".category-published").prop('selected', true);
		}
		else {
			$('#CategoryStatus').find(".category-pending").prop('selected', true);
		}

		//set order
		$('#CategoryOrder').find(".category-order").val(categoryOrder);
		$(".category-order").text(categoryOrder);

		//set form action
		$('.edit-category-action').attr('action', "EditCategory");

		//show edit modal
		$('.edit-category-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	$('.edit-slider').on('click', function (e) {
		//get clicked element key/id
		var sliderID = $(this).data("id");
		var sliderTitle = $(this).data('title');
		var sliderSubText = $(this).data('sub-text');
		var sliderLink = $(this).data('link');
		var sliderStatus = $(this).data('status');
		//var sliderImage = $(this).data('image');

		
		//check if main inputs are empty
		if (isEmpty(sliderID) || isEmpty(sliderTitle) || isEmpty(sliderStatus)) {
			alert("missng required data values");
			return false;
		}

		//set modal inputs
		$(".slider-id").val(sliderID);
		$(".slider-title").val(sliderTitle);
		$(".slider-sub-text").val(sliderSubText);
		$(".slider-link").val(sliderLink);
		//$(".slider-image-preview").attr("src", "/sliders/images/"+sliderImage);

		//set modal select options
		if ($.trim(sliderStatus) == '1') {
			$('#SliderStatus').find(".slider-published").prop('selected', true);
		}
		else {
			$('#SliderStatus').find(".slider-pending").prop('selected', true);
		}

		//show edit modal
		$('.edit-slider-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	$('.edit-service-stat').on('click', function (e) {
		//get clicked element key/id
		var serviceStatID = $(this).data("id");
		var serviceStatTitle = $(this).data('title');
		var serviceStatValue = $(this).data('stat-value');
		var serviceStatIcon = $(this).data('icon');
		var serviceStatLink = $(this).data('link');
		var serviceStatOrder = $(this).data('order');
		var serviceStatStatus = $(this).data('status');


		//check if main inputs are empty
		if (isEmpty(serviceStatID) || isEmpty(serviceStatTitle)) {
			alert("missng required data values");
			return false;
		}

		//set modal inputs
		$(".service-stat-id").val(serviceStatID);
		$(".service-stat-title").val(serviceStatTitle);
		$(".service-stat-value").val(serviceStatValue);
		$(".service-stat-icon").val(serviceStatIcon);
		$(".service-stat-link").val(serviceStatLink);

		//set modal select options
		if ($.trim(serviceStatStatus) == '1') {
			$('#ServiceStatStatus').find(".service-stat-published").prop('selected', true);
		}
		else {
			$('#ServiceStatStatus').find(".service-stat-pending").prop('selected', true);
		}

		//set order
		$('#ServiceStatOrder').find(".service-stat-order").val(serviceStatOrder);
		$(".service-stat-orderr").text(serviceStatOrder);
		$('.service-stat-orderr select option:contains("Select order")').text(serviceStatOrder);

		//show edit modal
		$('.edit-service-stat-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//clears new category form on modal click
	$('.clear-category-form').on('click', function (e) {
		//set modal inputs
		$(".category-id").val("");
		$(".category-name").val("");
		$(".short-Description").val("");
		$(".category-icon").val("");

		//set modal select options
		$('#CategoryStatus').find(".category-published").prop('selected', false);
		$('#CategoryStatus').find(".category-pending").prop('selected', false);

		//set form action
		$('.edit-category-action').attr('action', "AddCategory");
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	$('.edit-navigation').on('click', function (e) {
		//get clicked element key/id
		var navigationID = $(this).data("id");
		var navigationName = $(this).data('name');
		var navigationLink = $(this).data('link');
		var navigationParent = $(this).data('parent');
		var navigationParentName = $(this).data('parent-name');
		var navigationStatus = $(this).data('status');
		var navigationOrder = $(this).data('order');

		//check if main inputs are empty
		if (isEmpty(navigationID) || isEmpty(navigationName) || isEmpty(navigationStatus)) {
			alert("missng required data values");
			return false;
		}

		//set modal inputs
		$(".navigation-id").val(navigationID);
		$(".navigation-name").val(navigationName);
		$(".navigation-link").val(navigationLink);

		//if it has parent
		if (!isEmpty(navigationParent)) {
			$(".navigation-parent").val(navigationParent);
			$(".navigation-parent").html(navigationParentName);
			$('#NavigationParent').find(".navigation-parent").prop('selected', true);
        }

		//set modal select options
		if ($.trim(navigationStatus) == '1') {
			$('#NavigationStatus').find(".navigation-published").prop('selected', true);
		}
		else {
			$('#NavigationStatus').find(".navigation-pending").prop('selected', true);
		}

		$('#NavigationOrder').find(".navigation-order").val(navigationOrder);
		$(".navigation-order").text(navigationOrder);

		//set form action
		$('.edit-navigation-action').attr('action', "EditNavigation");

		//show edit modal
		$('.edit-navigation-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//clears new navigation form on modal click
	$('.clear-navigation-form').on('click', function (e) {
		//set modal inputs
		$(".navigation-id").val("");
		$(".navigation-name").val("");
		$(".navigation-link").val("");

		//set modal select options
		$('#NavigationStatus').find(".navigation-published").prop('selected', false);
		$('#NavigationStatus').find(".navigation-pending").prop('selected', false);
		$('#NavigationParent').find(".navigation-parent").prop('selected', false);

		//set form action
		$('.edit-navigation-action').attr('action', "AddNavigation");
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/
	//clears new category form on modal click
	$('.edit-account-role').on('click', function (e) {
		//get clicked element key/id
		var accountID = $(this).data("id");
		var authorRole = $(this).data('author');
		var editorRole = $(this).data('editor');
		var adminRole = $(this).data('admin');

		//set modal inputs
		$(".account-roles-id").val(accountID);

		//set roles checkboxes
		if (!isEmpty(authorRole) && $.trim(authorRole) === '1') {
			$(".account-roles-author").prop('checked', true);
		}

		if (!isEmpty(editorRole) && $.trim(editorRole) === '1') {
			$(".account-roles-editor").prop('checked', true);
		}

		if (!isEmpty(adminRole) && $.trim(adminRole) === '1') {
			$(".account-roles-admin").prop('checked', true);
		}

		//show edit modal
		$('.edit-account-roles-modal').modal('show');
	});

	/**
* ##################################################################################################################
* ##################################################################################################################
*/
	$('.edit-donation').on('click', function (e) {
		//get clicked element key/id
		var donationID = $(this).data("id");
		var donationTitle = $(this).data('title');
		var donationDescription = $(this).data('desc');
		var donationImage = $(this).data('image');
		var donationType= $(this).data('type');
		var donationLink = $(this).data('link');
		var donationStatus = $(this).data('status');


		//check if main inputs are empty
		if (isEmpty(donationID) || isEmpty(donationTitle) || isEmpty(donationType)) {
			alert("missng required data values");
			return false;
		}

		//set modal inputs
		$(".donation-id").val(donationID);
		$(".donation-title").val(donationTitle);
		$(".donation-description").val(donationDescription);
		//$(".donation-image").val(donationImage);
		//$(".donation-type").val(donationType);
		$(".donation-link").val(donationLink);

		//set modal select options
		if ($.trim(donationStatus) == '1') {
			$('#DonationCampaignStatus').find(".donation-published").prop('selected', true);
		}
		else {
			$('#DonationCampaignStatus').find(".donation-pending").prop('selected', true);
		}

		//show edit modal
		$('.edit-donation-campaign-modal').modal('show');
	});

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	    

	//Close modal class
	$(".close-modal").click(function () {
		$('.modal').modal('hide');
	});


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	

	//setting datatable
	$('#dataTable, .dataTable').DataTable();

	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	
	//prevent ancchor click scrolling top
	$('a.no-scroll').click(function(e) {
	    e.preventDefault();
	});


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	

	//class callback-input, used to display ajax call verification results after input field
	//use data-callback-type to know type of input, like 'email'


	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	
	// smooth scrolling
	$('.smooth-scroll')
		// Remove links that don't actually link to anything
		.not('[href="#"]')
		.not('[href="#0"]')
		.click(function (event) {
			// On-page links
			if (
				location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
				&&
				location.hostname == this.hostname
			) {
				// Figure out element to scroll to
				var target = $(this.hash);
				target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
				// Does a scroll target exist?
				if (target.length) {
					// Only prevent default if animation is actually gonna happen
					event.preventDefault();
					$('html, body').animate({
						scrollTop: target.offset().top
					}, 1000, function () {
						// Callback after animation
						// Must change focus!
						var $target = $(target);
						$target.focus();
						if ($target.is(":focus")) { // Checking if the target was focused
							return false;
						} else {
							$target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
							$target.focus(); // Set focus again
						};
					});
				}
			}
		});
		
	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	



	/**
	* ##################################################################################################################
	* ##################################################################################################################
	*/	
});


/**
* ##################################################################################################################
* ##################################################################################################################
*/	
$( document ).ready(function() {
	/* 
	  Switch input masks for passwor
	*/
	$('.unmask').on('click', function(){
		  var unmaskInput = $(this).data("unmask");
		  var inputType = $('#'+unmaskInput).attr('type');
		  if(inputType === 'text'){
		  	$('#'+unmaskInput).prop("type", "password");
		  }
		  else{
		  	$('#'+unmaskInput).prop("type", "text");
		  }
	});
});

/**
* ##################################################################################################################
* ##################################################################################################################
*/	
$( document ).ready(function() {
	// Restricts input for each element in the set of matched elements to the given inputFilter.
    (function ($) {
        $.fn.inputFilter = function (inputFilter) {
            return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    this.value = "";
                }
            });
        };
    }(jQuery));

    // set input filters.
    $(".integer-only").inputFilter(function (value) {
        return /^-?\d*$/.test(value);
    });

    $(".integer-plus-only").inputFilter(function (value) {
        return /^\d*$/.test(value);
    });

    $(".integer-range").inputFilter(function (value) {
        return /^\d*$/.test(value) && (value === "" || parseInt(value) <= 500);
    });

    $(".float-number").inputFilter(function (value) {
        return /^-?\d*[.,]?\d*$/.test(value);
    });

    $(".currency-number").inputFilter(function (value) {
        return /^-?\d*[.,]?\d{0,2}$/.test(value);
    });

    $(".latin-only").inputFilter(function (value) {
        return /^[a-z]*$/i.test(value);
    });

    $(".letters-only").inputFilter(function (value) {
        return /^[a-zA-Z ]*$/i.test(value);
    });

    $(".hex-text").inputFilter(function (value) {
        return /^[0-9a-f]*$/i.test(value);
    });

    //allows only decimals for input
    $('.decimal-only').keyup(function () {
        var val = $(this).val();
        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2)
                val = val.replace(/\.+$/, "");
        }
        $(this).val(val);
    });

	//letters only and space
    $('.alpha-only').bind('keyup blur', function () {
        var node = $(this);
        node.val(node.val().replace(/[^a-z. ]/g, ''));
	});

	//remove spaces from input
	$('.no-space').keyup(function () {
		var value = $(".no-space").val().replace(/\s/g, '');
		$(".no-space").val(value);
	});
}); 


/**
* ##################################################################################################################
* ##################################################################################################################
*/	
$( document ).ready(function() {
	 //dashboard search filter action
	$(".search-input-filter").on("keyup", function() {
	  var value = $(this).val().toLowerCase();
		
	  //check if value is empty or not
	  if(!isEmpty(value)){
		//show result div
	  	$('.search-list').addClass('d-block').removeClass('d-none');

	  	//add negative margin top class
	  	$('.search-result-div').addClass('minus-3');
	  }
	  else{
	  	//hide div
	  	$('.search-list').addClass('d-none').removeClass('d-block');

	  	//remove negative margin top class
	  	$('.search-result-div').removeClass('minus-3');
	  }

	  $(".search-list li").filter(function() {
	    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
	  });
	});
});  

/**
* ##################################################################################################################
* ##################################################################################################################
*/	
$( document ).ready(function() {
	//set imgae input accept types
	$('.accept-all-imgs').attr("accept", "image/*");
	$('.accept-imgs').attr("accept", "image/jpg,image/jpeg,image/png");
	$('.accept-jpgs').attr("accept", "image/jpg,image/jpeg");
	$('.accept-pngs').attr("accept", "image/png");
});

/**
* ##################################################################################################################
* ##################################################################################################################
*/	

//Check variable for empty
function isEmpty(value){
  return (value == null || value.length === 0);
}


//method to check if string contains text/character(s)
function containsString(string, text) {
	if (string.indexOf(text) > -1) {
		return true;
	}
	return false;
}





/*

	jQuery Tags Input Plugin 1.3.3

	Copyright (c) 2011 XOXCO, Inc

	Documentation for this plugin lives here:
	http://xoxco.com/clickable/jquery-tags-input

	Licensed under the MIT license:
	http://www.opensource.org/licenses/mit-license.php

	ben@xoxco.com

*/

(function($) {

	var delimiter = new Array();
	var tags_callbacks = new Array();
	$.fn.doAutosize = function(o){
	    var minWidth = $(this).data('minwidth'),
	        maxWidth = $(this).data('maxwidth'),
	        val = '',
	        input = $(this),
	        testSubject = $('#'+$(this).data('tester_id'));

	    if (val === (val = input.val())) {return;}

	    // Enter new content into testSubject
	    var escaped = val.replace(/&/g, '&amp;').replace(/\s/g,' ').replace(/</g, '&lt;').replace(/>/g, '&gt;');
	    testSubject.html(escaped);
	    // Calculate new width + whether to change
	    var testerWidth = testSubject.width(),
	        newWidth = (testerWidth + o.comfortZone) >= minWidth ? testerWidth + o.comfortZone : minWidth,
	        currentWidth = input.width(),
	        isValidWidthChange = (newWidth < currentWidth && newWidth >= minWidth)
	                             || (newWidth > minWidth && newWidth < maxWidth);

	    // Animate width
	    if (isValidWidthChange) {
	        input.width(newWidth);
	    }


  };
  $.fn.resetAutosize = function(options){
    // alert(JSON.stringify(options));
    var minWidth =  $(this).data('minwidth') || options.minInputWidth || $(this).width(),
        maxWidth = $(this).data('maxwidth') || options.maxInputWidth || ($(this).closest('.tagsinput').width() - options.inputPadding),
        val = '',
        input = $(this),
        testSubject = $('<tester/>').css({
            position: 'absolute',
            top: -9999,
            left: -9999,
            width: 'auto',
            fontSize: input.css('fontSize'),
            fontFamily: input.css('fontFamily'),
            fontWeight: input.css('fontWeight'),
            letterSpacing: input.css('letterSpacing'),
            whiteSpace: 'nowrap'
        }),
        testerId = $(this).attr('id')+'_autosize_tester';
    if(! $('#'+testerId).length > 0){
      testSubject.attr('id', testerId);
      testSubject.appendTo('body');
    }

    input.data('minwidth', minWidth);
    input.data('maxwidth', maxWidth);
    input.data('tester_id', testerId);
    input.css('width', minWidth);
  };

	$.fn.addTag = function(value,options) {
			options = jQuery.extend({focus:false,callback:true},options);
			this.each(function() {
				var id = $(this).attr('id');

				var tagslist = $(this).val().split(delimiter[id]);
				if (tagslist[0] == '') {
					tagslist = new Array();
				}

				value = jQuery.trim(value);

				if (options.unique) {
					var skipTag = $(this).tagExist(value);
					if(skipTag == true) {
					    //Marks fake input as not_valid to let styling it
    				    $('#'+id+'_tag').addClass('not_valid');
    				}
				} else {
					var skipTag = false;
				}

				if (value !='' && skipTag != true) {
                    $('<span>').addClass('tag').append(
                        $('<span>').text(value).append('&nbsp;&nbsp;'),
                        $('<a>', {
                            href  : '#',
                            title : 'Removing tag',
                            text  : 'x'
                        }).click(function () {
                            return $('#' + id).removeTag(escape(value));
                        })
                    ).insertBefore('#' + id + '_addTag');

					tagslist.push(value);

					$('#'+id+'_tag').val('');
					if (options.focus) {
						$('#'+id+'_tag').focus();
					} else {
						$('#'+id+'_tag').blur();
					}

					$.fn.tagsInput.updateTagsField(this,tagslist);

					if (options.callback && tags_callbacks[id] && tags_callbacks[id]['onAddTag']) {
						var f = tags_callbacks[id]['onAddTag'];
						f.call(this, value);
					}
					if(tags_callbacks[id] && tags_callbacks[id]['onChange'])
					{
						var i = tagslist.length;
						var f = tags_callbacks[id]['onChange'];
						f.call(this, $(this), tagslist[i-1]);
					}
				}

			});

			return false;
		};

	$.fn.removeTag = function(value) {
			value = unescape(value);
			this.each(function() {
				var id = $(this).attr('id');

				var old = $(this).val().split(delimiter[id]);

				$('#'+id+'_tagsinput .tag').remove();
				str = '';
				for (i=0; i< old.length; i++) {
					if (old[i]!=value) {
						str = str + delimiter[id] +old[i];
					}
				}

				$.fn.tagsInput.importTags(this,str);

				if (tags_callbacks[id] && tags_callbacks[id]['onRemoveTag']) {
					var f = tags_callbacks[id]['onRemoveTag'];
					f.call(this, value);
				}
			});

			return false;
		};

	$.fn.tagExist = function(val) {
		var id = $(this).attr('id');
		var tagslist = $(this).val().split(delimiter[id]);
		return (jQuery.inArray(val, tagslist) >= 0); //true when tag exists, false when not
	};

	// clear all existing tags and import new ones from a string
	$.fn.importTags = function(str) {
                id = $(this).attr('id');
		$('#'+id+'_tagsinput .tag').remove();
		$.fn.tagsInput.importTags(this,str);
	}

	$.fn.tagsInput = function(options) {
    var settings = jQuery.extend({
      interactive:true,
      defaultText:'add a tag',
      minChars:0,
      width:'300px',
      height:'100px',
      autocomplete: {selectFirst: false },
      'hide':true,
      'delimiter':',',
      'unique':true,
      removeWithBackspace:true,
      placeholderColor:'#666666',
      autosize: true,
      comfortZone: 20,
      inputPadding: 6*2
    },options);

		this.each(function() {
			if (settings.hide) {
				$(this).hide();
			}
			var id = $(this).attr('id');
			if (!id || delimiter[$(this).attr('id')]) {
				id = $(this).attr('id', 'tags' + new Date().getTime()).attr('id');
			}

			var data = jQuery.extend({
				pid:id,
				real_input: '#'+id,
				holder: '#'+id+'_tagsinput',
				input_wrapper: '#'+id+'_addTag',
				fake_input: '#'+id+'_tag'
			},settings);

			delimiter[id] = data.delimiter;

			if (settings.onAddTag || settings.onRemoveTag || settings.onChange) {
				tags_callbacks[id] = new Array();
				tags_callbacks[id]['onAddTag'] = settings.onAddTag;
				tags_callbacks[id]['onRemoveTag'] = settings.onRemoveTag;
				tags_callbacks[id]['onChange'] = settings.onChange;
			}

			var markup = '<div id="'+id+'_tagsinput" class="tagsinput"><div id="'+id+'_addTag">';

			if (settings.interactive) {
				markup = markup + '<input id="'+id+'_tag" value="" data-default="'+settings.defaultText+'" />';
			}

			markup = markup + '</div><div class="tags_clear"></div></div>';

			$(markup).insertAfter(this);

			$(data.holder).css('width',settings.width);
			$(data.holder).css('min-height',settings.height);
	
			if ($(data.real_input).val()!='') {
				$.fn.tagsInput.importTags($(data.real_input),$(data.real_input).val());
			}
			if (settings.interactive) {
				$(data.fake_input).val($(data.fake_input).attr('data-default'));
				$(data.fake_input).css('color',settings.placeholderColor);
		        $(data.fake_input).resetAutosize(settings);

				$(data.holder).bind('click',data,function(event) {
					$(event.data.fake_input).focus();
				});

				$(data.fake_input).bind('focus',data,function(event) {
					if ($(event.data.fake_input).val()==$(event.data.fake_input).attr('data-default')) {
						$(event.data.fake_input).val('');
					}
					$(event.data.fake_input).css('color','#000000');
				});

				if (settings.autocomplete_url != undefined) {
					autocomplete_options = {source: settings.autocomplete_url};
					for (attrname in settings.autocomplete) {
						autocomplete_options[attrname] = settings.autocomplete[attrname];
					}

					if (jQuery.Autocompleter !== undefined) {
						$(data.fake_input).autocomplete(settings.autocomplete_url, settings.autocomplete);
						$(data.fake_input).bind('result',data,function(event,data,formatted) {
							if (data) {
								$('#'+id).addTag(data[0] + "",{focus:true,unique:(settings.unique)});
							}
					  	});
					} else if (jQuery.ui.autocomplete !== undefined) {
						$(data.fake_input).autocomplete(autocomplete_options);
						$(data.fake_input).bind('autocompleteselect',data,function(event,ui) {
							$(event.data.real_input).addTag(ui.item.value,{focus:true,unique:(settings.unique)});
							return false;
						});
					}


				} else {
						// if a user tabs out of the field, create a new tag
						// this is only available if autocomplete is not used.
						$(data.fake_input).bind('blur',data,function(event) {
							var d = $(this).attr('data-default');
							if ($(event.data.fake_input).val()!='' && $(event.data.fake_input).val()!=d) {
								if( (event.data.minChars <= $(event.data.fake_input).val().length) && (!event.data.maxChars || (event.data.maxChars >= $(event.data.fake_input).val().length)) )
									$(event.data.real_input).addTag($(event.data.fake_input).val(),{focus:true,unique:(settings.unique)});
							} else {
								$(event.data.fake_input).val($(event.data.fake_input).attr('data-default'));
								$(event.data.fake_input).css('color',settings.placeholderColor);
							}
							return false;
						});

				}
				// if user types a comma, create a new tag
				$(data.fake_input).bind('keypress',data,function(event) {
					if (event.which==event.data.delimiter.charCodeAt(0) || event.which==13 ) {
					    event.preventDefault();
						if( (event.data.minChars <= $(event.data.fake_input).val().length) && (!event.data.maxChars || (event.data.maxChars >= $(event.data.fake_input).val().length)) )
							$(event.data.real_input).addTag($(event.data.fake_input).val(),{focus:true,unique:(settings.unique)});
					  	$(event.data.fake_input).resetAutosize(settings);
						return false;
					} else if (event.data.autosize) {
			            $(event.data.fake_input).doAutosize(settings);

          			}
				});
				//Delete last tag on backspace
				data.removeWithBackspace && $(data.fake_input).bind('keydown', function(event)
				{
					if(event.keyCode == 8 && $(this).val() == '')
					{
						 event.preventDefault();
						 var last_tag = $(this).closest('.tagsinput').find('.tag:last').text();
						 var id = $(this).attr('id').replace(/_tag$/, '');
						 last_tag = last_tag.replace(/[\s]+x$/, '');
						 $('#' + id).removeTag(escape(last_tag));
						 $(this).trigger('focus');
					}
				});
				$(data.fake_input).blur();

				//Removes the not_valid class when user changes the value of the fake input
				if(data.unique) {
				    $(data.fake_input).keydown(function(event){
				        if(event.keyCode == 8 || String.fromCharCode(event.which).match(/\w+|[áéíóúÁÉÍÓÚñÑ,/]+/)) {
				            $(this).removeClass('not_valid');
				        }
				    });
				}
			} // if settings.interactive
		});

		return this;

	};

	$.fn.tagsInput.updateTagsField = function(obj,tagslist) {
		var id = $(obj).attr('id');
		$(obj).val(tagslist.join(delimiter[id]));
	};

	$.fn.tagsInput.importTags = function(obj,val) {
		$(obj).val('');
		var id = $(obj).attr('id');
		var tags = val.split(delimiter[id]);
		for (i=0; i<tags.length; i++) {
			$(obj).addTag(tags[i],{focus:false,callback:false});
		}
		if(tags_callbacks[id] && tags_callbacks[id]['onChange'])
		{
			var f = tags_callbacks[id]['onChange'];
			f.call(obj, obj, tags[i]);
		}
	};

})(jQuery);

//set tags input
$(function() {
    $('.tags-input').tagsInput({width:'auto'});
});