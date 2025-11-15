<%@ Page Title="Add User" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="SciVerse_G12.AddUser" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Bootstrap & FontAwesome Links -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <!-- Page-specific CSS -->
    <link href='<%= ResolveUrl("~/Styles/Admin.css?v=" + DateTime.Now.Ticks) %>' rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="add-user-container">
        <div class="add-user-card">
            <h1 class="add-user-header text-center mb-4">Add New User</h1>

            <!-- PERSONAL DETAILS -->
            <h4 class="section-title">Personal Details</h4>
            <div class="row mb-4 add-user-form">
                <div class="col-md-7">
                    <div class="mb-3">
                        <label class="form-label">Full Name</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Age</label>
                        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" Required="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Gender</label>
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" Required="true">
                            <asp:ListItem Text="Select Gender" Value=""></asp:ListItem>
                            <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                            <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Country</label>
                        <asp:DropDownList ID="dlCountry" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Albania">Albania</asp:ListItem>
                            <asp:ListItem Value="Algeria">Algeria</asp:ListItem>
                            <asp:ListItem Value="American Samoa">American Samoa</asp:ListItem>
                            <asp:ListItem Value="Andorra">Andorra</asp:ListItem>
                            <asp:ListItem Value="Angola">Angola</asp:ListItem>
                            <asp:ListItem Value="Anguilla">Anguilla</asp:ListItem>
                            <asp:ListItem Value="Antarctica">Antarctica</asp:ListItem>
                            <asp:ListItem Value="Antigua and Barbuda">Antigua and Barbuda</asp:ListItem>
                            <asp:ListItem Value="Argentina">Argentina</asp:ListItem>
                            <asp:ListItem Value="Armenia">Armenia</asp:ListItem>
                            <asp:ListItem Value="Aruba">Aruba</asp:ListItem>
                            <asp:ListItem Value="Australia">Australia</asp:ListItem>
                            <asp:ListItem Value="Austria">Austria</asp:ListItem>
                            <asp:ListItem Value="Azerbaijan">Azerbaijan</asp:ListItem>
                            <asp:ListItem Value="Bahamas">Bahamas</asp:ListItem>
                            <asp:ListItem Value="Bahrain">Bahrain</asp:ListItem>
                            <asp:ListItem Value="Bangladesh">Bangladesh</asp:ListItem>
                            <asp:ListItem Value="Barbados">Barbados</asp:ListItem>
                            <asp:ListItem Value="Belarus">Belarus</asp:ListItem>
                            <asp:ListItem Value="Belgium">Belgium</asp:ListItem>
                            <asp:ListItem Value="Belize">Belize</asp:ListItem>
                            <asp:ListItem Value="Benin">Benin</asp:ListItem>
                            <asp:ListItem Value="Bermuda">Bermuda</asp:ListItem>
                            <asp:ListItem Value="Bhutan">Bhutan</asp:ListItem>
                            <asp:ListItem Value="Bolivia">Bolivia</asp:ListItem>
                            <asp:ListItem Value="Bosnia and Herzegovina">Bosnia and Herzegovina</asp:ListItem>
                            <asp:ListItem Value="Botswana">Botswana</asp:ListItem>
                            <asp:ListItem Value="Bouvet Island">Bouvet Island</asp:ListItem>
                            <asp:ListItem Value="Brazil">Brazil</asp:ListItem>
                            <asp:ListItem Value="British Indian Ocean Territory">British Indian Ocean Territory</asp:ListItem>
                            <asp:ListItem Value="Brunei Darussalam">Brunei Darussalam</asp:ListItem>
                            <asp:ListItem Value="Bulgaria">Bulgaria</asp:ListItem>
                            <asp:ListItem Value="Burkina Faso">Burkina Faso</asp:ListItem>
                            <asp:ListItem Value="Burundi">Burundi</asp:ListItem>
                            <asp:ListItem Value="Cambodia">Cambodia</asp:ListItem>
                            <asp:ListItem Value="Cameroon">Cameroon</asp:ListItem>
                            <asp:ListItem Value="Canada">Canada</asp:ListItem>
                            <asp:ListItem Value="Cape Verde">Cape Verde</asp:ListItem>
                            <asp:ListItem Value="Cayman Islands">Cayman Islands</asp:ListItem>
                            <asp:ListItem Value="Central African Republic">Central African Republic</asp:ListItem>
                            <asp:ListItem Value="Chad">Chad</asp:ListItem>
                            <asp:ListItem Value="Chile">Chile</asp:ListItem>
                            <asp:ListItem Value="China">China</asp:ListItem>
                            <asp:ListItem Value="Christmas Island">Christmas Island</asp:ListItem>
                            <asp:ListItem Value="Cocos (Keeling) Islands">Cocos (Keeling) Islands</asp:ListItem>
                            <asp:ListItem Value="Colombia">Colombia</asp:ListItem>
                            <asp:ListItem Value="Comoros">Comoros</asp:ListItem>
                            <asp:ListItem Value="Congo">Congo</asp:ListItem>
                            <asp:ListItem Value="Congo, The Democratic Republic of The">Congo, The Democratic Republic of The</asp:ListItem>
                            <asp:ListItem Value="Cook Islands">Cook Islands</asp:ListItem>
                            <asp:ListItem Value="Costa Rica">Costa Rica</asp:ListItem>
                            <asp:ListItem Value="Cote D'ivoire">Cote D'ivoire</asp:ListItem>
                            <asp:ListItem Value="Croatia">Croatia</asp:ListItem>
                            <asp:ListItem Value="Cuba">Cuba</asp:ListItem>
                            <asp:ListItem Value="Cyprus">Cyprus</asp:ListItem>
                            <asp:ListItem Value="Czech Republic">Czech Republic</asp:ListItem>
                            <asp:ListItem Value="Denmark">Denmark</asp:ListItem>
                            <asp:ListItem Value="Djibouti">Djibouti</asp:ListItem>
                            <asp:ListItem Value="Dominica">Dominica</asp:ListItem>
                            <asp:ListItem Value="Dominican Republic">Dominican Republic</asp:ListItem>
                            <asp:ListItem Value="Ecuador">Ecuador</asp:ListItem>
                            <asp:ListItem Value="Egypt">Egypt</asp:ListItem>
                            <asp:ListItem Value="El Salvador">El Salvador</asp:ListItem>
                            <asp:ListItem Value="Equatorial Guinea">Equatorial Guinea</asp:ListItem>
                            <asp:ListItem Value="Eritrea">Eritrea</asp:ListItem>
                            <asp:ListItem Value="Estonia">Estonia</asp:ListItem>
                            <asp:ListItem Value="Ethiopia">Ethiopia</asp:ListItem>
                            <asp:ListItem Value="Falkland Islands (Malvinas)">Falkland Islands (Malvinas)</asp:ListItem>
                            <asp:ListItem Value="Faroe Islands">Faroe Islands</asp:ListItem>
                            <asp:ListItem Value="Fiji">Fiji</asp:ListItem>
                            <asp:ListItem Value="Finland">Finland</asp:ListItem>
                            <asp:ListItem Value="France">France</asp:ListItem>
                            <asp:ListItem Value="French Guiana">French Guiana</asp:ListItem>
                            <asp:ListItem Value="French Polynesia">French Polynesia</asp:ListItem>
                            <asp:ListItem Value="French Southern Territories">French Southern Territories</asp:ListItem>
                            <asp:ListItem Value="Gabon">Gabon</asp:ListItem>
                            <asp:ListItem Value="Gambia">Gambia</asp:ListItem>
                            <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                            <asp:ListItem Value="Germany">Germany</asp:ListItem>
                            <asp:ListItem Value="Ghana">Ghana</asp:ListItem>
                            <asp:ListItem Value="Gibraltar">Gibraltar</asp:ListItem>
                            <asp:ListItem Value="Greece">Greece</asp:ListItem>
                            <asp:ListItem Value="Greenland">Greenland</asp:ListItem>
                            <asp:ListItem Value="Grenada">Grenada</asp:ListItem>
                            <asp:ListItem Value="Guadeloupe">Guadeloupe</asp:ListItem>
                            <asp:ListItem Value="Guam">Guam</asp:ListItem>
                            <asp:ListItem Value="Guatemala">Guatemala</asp:ListItem>
                            <asp:ListItem Value="Guernsey">Guernsey</asp:ListItem>
                            <asp:ListItem Value="Guinea">Guinea</asp:ListItem>
                            <asp:ListItem Value="Guinea-bissau">Guinea-bissau</asp:ListItem>
                            <asp:ListItem Value="Guyana">Guyana</asp:ListItem>
                            <asp:ListItem Value="Haiti">Haiti</asp:ListItem>
                            <asp:ListItem Value="Heard Island and Mcdonald Islands">Heard Island and Mcdonald Islands</asp:ListItem>
                            <asp:ListItem Value="Holy See (Vatican City State)">Holy See (Vatican City State)</asp:ListItem>
                            <asp:ListItem Value="Honduras">Honduras</asp:ListItem>
                            <asp:ListItem Value="Hong Kong">Hong Kong</asp:ListItem>
                            <asp:ListItem Value="Hungary">Hungary</asp:ListItem>
                            <asp:ListItem Value="Iceland">Iceland</asp:ListItem>
                            <asp:ListItem Value="India">India</asp:ListItem>
                            <asp:ListItem Value="Indonesia">Indonesia</asp:ListItem>
                            <asp:ListItem Value="Iran, Islamic Republic of">Iran, Islamic Republic of</asp:ListItem>
                            <asp:ListItem Value="Iraq">Iraq</asp:ListItem>
                            <asp:ListItem Value="Ireland">Ireland</asp:ListItem>
                            <asp:ListItem Value="Isle of Man">Isle of Man</asp:ListItem>
                            <asp:ListItem Value="Israel">Israel</asp:ListItem>
                            <asp:ListItem Value="Italy">Italy</asp:ListItem>
                            <asp:ListItem Value="Jamaica">Jamaica</asp:ListItem>
                            <asp:ListItem Value="Japan">Japan</asp:ListItem>
                            <asp:ListItem Value="Jersey">Jersey</asp:ListItem>
                            <asp:ListItem Value="Jordan">Jordan</asp:ListItem>
                            <asp:ListItem Value="Kazakhstan">Kazakhstan</asp:ListItem>
                            <asp:ListItem Value="Kenya">Kenya</asp:ListItem>
                            <asp:ListItem Value="Kiribati">Kiribati</asp:ListItem>
                            <asp:ListItem Value="Korea, Democratic People's Republic of">Korea, Democratic People's Republic of</asp:ListItem>
                            <asp:ListItem Value="Korea, Republic of">Korea, Republic of</asp:ListItem>
                            <asp:ListItem Value="Kuwait">Kuwait</asp:ListItem>
                            <asp:ListItem Value="Kyrgyzstan">Kyrgyzstan</asp:ListItem>
                            <asp:ListItem Value="Lao People's Democratic Republic">Lao People's Democratic Republic</asp:ListItem>
                            <asp:ListItem Value="Latvia">Latvia</asp:ListItem>
                            <asp:ListItem Value="Lebanon">Lebanon</asp:ListItem>
                            <asp:ListItem Value="Lesotho">Lesotho</asp:ListItem>
                            <asp:ListItem Value="Liberia">Liberia</asp:ListItem>
                            <asp:ListItem Value="Libyan Arab Jamahiriya">Libyan Arab Jamahiriya</asp:ListItem>
                            <asp:ListItem Value="Liechtenstein">Liechtenstein</asp:ListItem>
                            <asp:ListItem Value="Lithuania">Lithuania</asp:ListItem>
                            <asp:ListItem Value="Luxembourg">Luxembourg</asp:ListItem>
                            <asp:ListItem Value="Macao">Macao</asp:ListItem>
                            <asp:ListItem Value="Macedonia, The Former Yugoslav Republic of">Macedonia, The Former Yugoslav Republic of</asp:ListItem>
                            <asp:ListItem Value="Madagascar">Madagascar</asp:ListItem>
                            <asp:ListItem Value="Malawi">Malawi</asp:ListItem>
                            <asp:ListItem Value="Malaysia">Malaysia</asp:ListItem>
                            <asp:ListItem Value="Maldives">Maldives</asp:ListItem>
                            <asp:ListItem Value="Mali">Mali</asp:ListItem>
                            <asp:ListItem Value="Malta">Malta</asp:ListItem>
                            <asp:ListItem Value="Marshall Islands">Marshall Islands</asp:ListItem>
                            <asp:ListItem Value="Martinique">Martinique</asp:ListItem>
                            <asp:ListItem Value="Mauritania">Mauritania</asp:ListItem>
                            <asp:ListItem Value="Mauritius">Mauritius</asp:ListItem>
                            <asp:ListItem Value="Mayotte">Mayotte</asp:ListItem>
                            <asp:ListItem Value="Mexico">Mexico</asp:ListItem>
                            <asp:ListItem Value="Micronesia, Federated States of">Micronesia, Federated States of</asp:ListItem>
                            <asp:ListItem Value="Moldova, Republic of">Moldova, Republic of</asp:ListItem>
                            <asp:ListItem Value="Monaco">Monaco</asp:ListItem>
                            <asp:ListItem Value="Mongolia">Mongolia</asp:ListItem>
                            <asp:ListItem Value="Montenegro">Montenegro</asp:ListItem>
                            <asp:ListItem Value="Montserrat">Montserrat</asp:ListItem>
                            <asp:ListItem Value="Morocco">Morocco</asp:ListItem>
                            <asp:ListItem Value="Mozambique">Mozambique</asp:ListItem>
                            <asp:ListItem Value="Myanmar">Myanmar</asp:ListItem>
                            <asp:ListItem Value="Namibia">Namibia</asp:ListItem>
                            <asp:ListItem Value="Nauru">Nauru</asp:ListItem>
                            <asp:ListItem Value="Nepal">Nepal</asp:ListItem>
                            <asp:ListItem Value="Netherlands">Netherlands</asp:ListItem>
                            <asp:ListItem Value="Netherlands Antilles">Netherlands Antilles</asp:ListItem>
                            <asp:ListItem Value="New Caledonia">New Caledonia</asp:ListItem>
                            <asp:ListItem Value="New Zealand">New Zealand</asp:ListItem>
                            <asp:ListItem Value="Nicaragua">Nicaragua</asp:ListItem>
                            <asp:ListItem Value="Niger">Niger</asp:ListItem>
                            <asp:ListItem Value="Nigeria">Nigeria</asp:ListItem>
                            <asp:ListItem Value="Niue">Niue</asp:ListItem>
                            <asp:ListItem Value="Norfolk Island">Norfolk Island</asp:ListItem>
                            <asp:ListItem Value="Northern Mariana Islands">Northern Mariana Islands</asp:ListItem>
                            <asp:ListItem Value="Norway">Norway</asp:ListItem>
                            <asp:ListItem Value="Oman">Oman</asp:ListItem>
                            <asp:ListItem Value="Pakistan">Pakistan</asp:ListItem>
                            <asp:ListItem Value="Palau">Palau</asp:ListItem>
                            <asp:ListItem Value="Palestinian Territory, Occupied">Palestinian Territory, Occupied</asp:ListItem>
                            <asp:ListItem Value="Panama">Panama</asp:ListItem>
                            <asp:ListItem Value="Papua New Guinea">Papua New Guinea</asp:ListItem>
                            <asp:ListItem Value="Paraguay">Paraguay</asp:ListItem>
                            <asp:ListItem Value="Peru">Peru</asp:ListItem>
                            <asp:ListItem Value="Philippines">Philippines</asp:ListItem>
                            <asp:ListItem Value="Pitcairn">Pitcairn</asp:ListItem>
                            <asp:ListItem Value="Poland">Poland</asp:ListItem>
                            <asp:ListItem Value="Portugal">Portugal</asp:ListItem>
                            <asp:ListItem Value="Puerto Rico">Puerto Rico</asp:ListItem>
                            <asp:ListItem Value="Qatar">Qatar</asp:ListItem>
                            <asp:ListItem Value="Reunion">Reunion</asp:ListItem>
                            <asp:ListItem Value="Romania">Romania</asp:ListItem>
                            <asp:ListItem Value="Russian Federation">Russian Federation</asp:ListItem>
                            <asp:ListItem Value="Rwanda">Rwanda</asp:ListItem>
                            <asp:ListItem Value="Saint Helena">Saint Helena</asp:ListItem>
                            <asp:ListItem Value="Saint Kitts and Nevis">Saint Kitts and Nevis</asp:ListItem>
                            <asp:ListItem Value="Saint Lucia">Saint Lucia</asp:ListItem>
                            <asp:ListItem Value="Saint Pierre and Miquelon">Saint Pierre and Miquelon</asp:ListItem>
                            <asp:ListItem Value="Saint Vincent and The Grenadines">Saint Vincent and The Grenadines</asp:ListItem>
                            <asp:ListItem Value="Samoa">Samoa</asp:ListItem>
                            <asp:ListItem Value="San Marino">San Marino</asp:ListItem>
                            <asp:ListItem Value="Sao Tome and Principe">Sao Tome and Principe</asp:ListItem>
                            <asp:ListItem Value="Saudi Arabia">Saudi Arabia</asp:ListItem>
                            <asp:ListItem Value="Senegal">Senegal</asp:ListItem>
                            <asp:ListItem Value="Serbia">Serbia</asp:ListItem>
                            <asp:ListItem Value="Seychelles">Seychelles</asp:ListItem>
                            <asp:ListItem Value="Sierra Leone">Sierra Leone</asp:ListItem>
                            <asp:ListItem Value="Singapore">Singapore</asp:ListItem>
                            <asp:ListItem Value="Slovakia">Slovakia</asp:ListItem>
                            <asp:ListItem Value="Slovenia">Slovenia</asp:ListItem>
                            <asp:ListItem Value="Solomon Islands">Solomon Islands</asp:ListItem>
                            <asp:ListItem Value="Somalia">Somalia</asp:ListItem>
                            <asp:ListItem Value="South Africa">South Africa</asp:ListItem>
                            <asp:ListItem Value="South Georgia and The South Sandwich Islands">South Georgia and The South Sandwich Islands</asp:ListItem>
                            <asp:ListItem Value="Spain">Spain</asp:ListItem>
                            <asp:ListItem Value="Sri Lanka">Sri Lanka</asp:ListItem>
                            <asp:ListItem Value="Sudan">Sudan</asp:ListItem>
                            <asp:ListItem Value="Suriname">Suriname</asp:ListItem>
                            <asp:ListItem Value="Svalbard and Jan Mayen">Svalbard and Jan Mayen</asp:ListItem>
                            <asp:ListItem Value="Swaziland">Swaziland</asp:ListItem>
                            <asp:ListItem Value="Sweden">Sweden</asp:ListItem>
                            <asp:ListItem Value="Switzerland">Switzerland</asp:ListItem>
                            <asp:ListItem Value="Syrian Arab Republic">Syrian Arab Republic</asp:ListItem>
                            <asp:ListItem Value="Taiwan">Taiwan</asp:ListItem>
                            <asp:ListItem Value="Tajikistan">Tajikistan</asp:ListItem>
                            <asp:ListItem Value="Tanzania, United Republic of">Tanzania, United Republic of</asp:ListItem>
                            <asp:ListItem Value="Thailand">Thailand</asp:ListItem>
                            <asp:ListItem Value="Timor-leste">Timor-leste</asp:ListItem>
                            <asp:ListItem Value="Togo">Togo</asp:ListItem>
                            <asp:ListItem Value="Tokelau">Tokelau</asp:ListItem>
                            <asp:ListItem Value="Tonga">Tonga</asp:ListItem>
                            <asp:ListItem Value="Trinidad and Tobago">Trinidad and Tobago</asp:ListItem>
                            <asp:ListItem Value="Tunisia">Tunisia</asp:ListItem>
                            <asp:ListItem Value="Turkey">Turkey</asp:ListItem>
                            <asp:ListItem Value="Turkmenistan">Turkmenistan</asp:ListItem>
                            <asp:ListItem Value="Turks and Caicos Islands">Turks and Caicos Islands</asp:ListItem>
                            <asp:ListItem Value="Tuvalu">Tuvalu</asp:ListItem>
                            <asp:ListItem Value="Uganda">Uganda</asp:ListItem>
                            <asp:ListItem Value="Ukraine">Ukraine</asp:ListItem>
                            <asp:ListItem Value="United Arab Emirates">United Arab Emirates</asp:ListItem>
                            <asp:ListItem Value="United Kingdom">United Kingdom</asp:ListItem>
                            <asp:ListItem Value="United States">United States</asp:ListItem>
                            <asp:ListItem Value="United States Minor Outlying Islands">United States Minor Outlying Islands</asp:ListItem>
                            <asp:ListItem Value="Uruguay">Uruguay</asp:ListItem>
                            <asp:ListItem Value="Uzbekistan">Uzbekistan</asp:ListItem>
                            <asp:ListItem Value="Vanuatu">Vanuatu</asp:ListItem>
                            <asp:ListItem Value="Venezuela">Venezuela</asp:ListItem>
                            <asp:ListItem Value="Viet Nam">Viet Nam</asp:ListItem>
                            <asp:ListItem Value="Virgin Islands, British">Virgin Islands, British</asp:ListItem>
                            <asp:ListItem Value="Virgin Islands, U.S.">Virgin Islands, U.S.</asp:ListItem>
                            <asp:ListItem Value="Wallis and Futuna">Wallis and Futuna</asp:ListItem>
                            <asp:ListItem Value="Western Sahara">Western Sahara</asp:ListItem>
                            <asp:ListItem Value="Yemen">Yemen</asp:ListItem>
                            <asp:ListItem Value="Zambia">Zambia</asp:ListItem>
                            <asp:ListItem Value="Zimbabwe">Zimbabwe</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-5 text-center image-preview-box">
                    <img id="imgPreview" src="../Images/default_profile.png" class="img-preview" alt="Profile Preview" />
                    <div class="mt-2">
                        <asp:FileUpload ID="fileUploadPicture" runat="server" CssClass="form-control" accept="image/*" onchange="showPreview(event)" />
                        <asp:RequiredFieldValidator runat="server" 
                                                    ControlToValidate="fileUploadPicture" 
                                                    CssClass="text-danger" 
                                                    ErrorMessage="Profile image is required." 
                                                    Display="Dynamic" 
                                                    InitialValue="" />
                        <asp:RegularExpressionValidator runat="server" 
                                                        ControlToValidate="fileUploadPicture" 
                                                        CssClass="text-danger" 
                                                        ErrorMessage="Only image files (JPG, PNG, GIF) are allowed." 
                                                        Display="Dynamic" 
                                                        ValidationExpression="^.+(\.jpg|\.jpeg|\.png|\.gif|\.bmp)$" />
                    </div>
                </div>
            </div>

            <!-- ACCOUNT DETAILS -->
            <h4 class="section-title">Account Details</h4>
            <div class="row add-user-form">
                <div class="col-md-6">
                    <label class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
                <div class="col-md-6">
                    <label class="form-label">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" Required="true"></asp:TextBox>
                </div>
            </div>
            <div class="row add-user-form">
                <div class="col-md-6">
                    <label class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Required="true"></asp:TextBox>
                    <asp:RegularExpressionValidator runat="server" 
                                                    ControlToValidate="txtPassword" 
                                                    CssClass="text-danger" 
                                                    ErrorMessage="Password must be at least 4 characters." 
                                                    Display="Dynamic" 
                                                    ValidationExpression="^.{4,}$" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select" Required="true">
                        <asp:ListItem Text="Select Role" Value=""></asp:ListItem>
                        <asp:ListItem Text="User" Value="User" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="lblMessage"></asp:Label>

            <div class="text-center mt-4">
                <asp:Button ID="btnSave" runat="server" CssClass="btn-save-user" Text="Save User" OnClick="btnSave_Click" />
            </div>

            <div class="text-center mt-3">
                <a href="ViewUserList.aspx" class="btn-back">Back to User List</a>
            </div>
        </div>
    </div>

    <script>
        function showPreview(event) {
            const preview = document.getElementById("imgPreview");
            if (event.target.files && event.target.files[0]) {
                preview.src = URL.createObjectURL(event.target.files[0]);
            }
        }
    </script>
</asp:Content>