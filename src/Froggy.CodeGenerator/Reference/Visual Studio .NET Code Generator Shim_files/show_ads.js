(function(){
var g=false,i=null,k=true,n=(new Date).getTime();var aa={google_ad_channel:"channel",google_ad_host:"host",google_ad_host_channel:"h_ch",google_ad_host_tier_id:"ht_id",google_ad_region:"region",google_ad_section:"region",google_ad_type:"ad_type",google_adtest:"adtest",google_allow_expandable_ads:"ea",google_alternate_ad_url:"alternate_ad_url",google_alternate_color:"alt_color",google_bid:"bid",google_city:"gcs",google_color_bg:"color_bg",google_color_border:"color_border",google_color_line:"color_line",google_color_link:"color_link",google_color_text:"color_text",
google_color_url:"color_url",google_contents:"contents",google_country:"gl",google_cust_age:"cust_age",google_cust_ch:"cust_ch",google_cust_gender:"cust_gender",google_cust_id:"cust_id",google_cust_interests:"cust_interests",google_cust_job:"cust_job",google_cust_l:"cust_l",google_cust_lh:"cust_lh",google_cust_u_url:"cust_u_url",google_disable_video_autoplay:"disable_video_autoplay",google_ed:"ed",google_encoding:"oe",google_feedback:"feedback_link",google_flash_version:"flash",google_font_face:"f",
google_gl:"gl",google_hints:"hints",google_kw:"kw",google_kw_type:"kw_type",google_language:"hl",google_page_url:"url",google_referrer_url:"ref",google_region:"gr",google_reuse_colors:"reuse_colors",google_safe:"adsafe",google_tag_info:"gut",google_targeting:"targeting",google_ui_features:"ui",google_ui_version:"uiv",google_video_doc_id:"video_doc_id",google_video_product_type:"video_product_type"},ba={google_ad_format:"format",google_ad_output:"output",google_ad_callback:"callback",google_ad_override:"google_ad_override",
google_ad_slot:"slotname",google_analytics_uacct:"ga_wpids",google_correlator:"correlator",google_cpa_choice:"cpa_choice",google_ctr_threshold:"ctr_t",google_image_size:"image_size",google_last_modified_time:"lmt",google_max_num_ads:"num_ads",google_max_radlink_len:"max_radlink_len",google_num_radlinks:"num_radlinks",google_num_radlinks_per_unit:"num_radlinks_per_unit",google_only_ads_with_video:"only_ads_with_video",google_page_location:"loc",google_rl_dest_url:"rl_dest_url",google_rl_filtering:"rl_filtering",
google_rl_mode:"rl_mode",google_rt:"rt",google_skip:"skip"},ca={google_only_pyv_ads:"pyv"};function o(a){return aa[a]||ba[a]||ca[a]||i};document.URL&&(document.URL.indexOf("?google_debug")>0||document.URL.indexOf("&google_debug")>0);function p(){this.b=this.p();this.h=g;if(!this.b)this.h=this.j()}p.prototype.f="__gads=";p.prototype.c="GoogleAdServingTest=";p.prototype.o=function(){return this.b};p.prototype.setCookieInfo=function(a){this.a=a._cookies_[0];if(this.a!=i){this.b=this.a._value_;this.q()}};p.prototype.l=function(a){var b=(new Date).valueOf(),c=new Date;c.setTime(b+a);return c};
p.prototype.k=function(a){if(!(this.b!=i||!this.h)){var b="script",c=document.domain,d="http://partner.googleadservices.com/gampad/cookie.js?callback=_GA_googleCookieHelper.setCookieInfo&client="+q(a)+"&domain="+q(c);document.write("<"+b+' src="'+d+'"></'+b+">")}};p.prototype.j=function(){document.cookie=this.c+"Good";var a=this.g(this.c),b=a=="Good";if(b){var c=this.l(-1);document.cookie=this.c+"; expires="+c.toGMTString()}return b};p.prototype.p=function(){var a=this.g(this.f);return a};
p.prototype.g=function(a){var b=document.cookie,c=b.indexOf(a),d=i;if(c!=-1){var e=c+a.length,f=b.indexOf(";",e);if(f==-1)f=b.length;d=b.substring(e,f)}return d};p.prototype.q=function(){if(!(this.a==i))if(!(this.b==i)){var a=new Date;a.setTime(1000*this.a._expires_);var b=this.a._domain_,c=this.f+this.b+"; expires="+a.toGMTString()+"; path="+this.a._path_+"; domain=."+b;document.cookie=c}};function r(a,b){var c;return a?(c=parseFloat(a))?c:b:b}
function da(a,b){if(a=="true")return k;if(a=="false")return g;return b}
function ea(){if(navigator.plugins&&navigator.mimeTypes.length){var a=navigator.plugins["Shockwave Flash"];if(a&&a.description)return a.description.replace(/([a-zA-Z]|\s)+/,"").replace(/(\s)+r/,".")}else if(navigator.userAgent&&navigator.userAgent.indexOf("Windows CE")>=0){var b=3,c=1;for(;c;)try{c=new ActiveXObject("ShockwaveFlash.ShockwaveFlash."+(b+1));b++}catch(d){c=i}return b.toString()}else if(fa()){var c=i;try{c=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.7")}catch(e){var b=0;try{c=new ActiveXObject("ShockwaveFlash.ShockwaveFlash.6");
b=6;c.AllowScriptAccess="always"}catch(f){if(b==6)return b.toString()}try{c=new ActiveXObject("ShockwaveFlash.ShockwaveFlash")}catch(h){}}if(c!=i){var b=c.GetVariable("$version").split(" ")[1];return b.replace(/,/g,".")}}return"0"};var u=this,x=function(a){var b=typeof a;if(b=="object")if(a){if(a instanceof Array||!(a instanceof Object)&&Object.prototype.toString.call(a)=="[object Array]")return"array";if(typeof a.call!="undefined")return"function"}else return"null";else if(b=="function"&&typeof a.call=="undefined")return"object";return b};var ga=function(a){var b=x(a);return b=="array"||b=="object"&&typeof a.length=="number"};
var ha=function(a){var b=x(a);return b=="object"||b=="array"||b=="function"},ia=function(a){var b=x(a);if(b=="object"||b=="array"){if(a.clone)return a.clone.call(a);var c=b=="array"?[]:{};for(var d in a)c[d]=ia(a[d]);return c}return a},ja=Date.now||function(){return(new Date).getTime()};var ka=function(a,b,c){if(a.forEach)a.forEach(b,c);else if(Array.forEach)Array.forEach(a,b,c);else{var d=a.length,e=typeof a=="string"?a.split(""):a,f=0;for(;f<d;f++)f in e&&b.call(c,e[f],f,a)}},la=function(a){if(x(a)=="array")return a.concat();else{var b=[],c=0,d=a.length;for(;c<d;c++)b[c]=a[c];return b}};var y=function(a,b){this.x=typeof a!="undefined"?a:0;this.y=typeof b!="undefined"?b:0};y.prototype.clone=function(){return new y(this.x,this.y)};y.prototype.toString=function(){return"("+this.x+", "+this.y+")"};var z=function(a,b){this.width=a;this.height=b};z.prototype.clone=function(){return new z(this.width,this.height)};z.prototype.toString=function(){return"("+this.width+" x "+this.height+")"};z.prototype.ceil=function(){this.width=Math.ceil(this.width);this.height=Math.ceil(this.height);return this};z.prototype.floor=function(){this.width=Math.floor(this.width);this.height=Math.floor(this.height);return this};
z.prototype.round=function(){this.width=Math.round(this.width);this.height=Math.round(this.height);return this};z.prototype.scale=function(a){this.width*=a;this.height*=a;return this};var pa=function(a,b,c){for(var d in a)b.call(c,a[d],d,a)};var qa=function(a){return a.replace(/^[\s\xa0]+|[\s\xa0]+$/g,"")},wa=function(a,b){if(b)return a.replace(ra,"&amp;").replace(sa,"&lt;").replace(ta,"&gt;").replace(ua,"&quot;");else{if(!va.test(a))return a;if(a.indexOf("&")!=-1)a=a.replace(ra,"&amp;");if(a.indexOf("<")!=-1)a=a.replace(sa,"&lt;");if(a.indexOf(">")!=-1)a=a.replace(ta,"&gt;");if(a.indexOf('"')!=-1)a=a.replace(ua,"&quot;");return a}},ra=/&/g,sa=/</g,ta=/>/g,ua=/\"/g,va=/[&<>\"]/,xa=function(a,b){var c=b.length,d=0;for(;d<c;d++){var e=
c==1?b:b.charAt(d);if(a.charAt(0)==e&&a.charAt(a.length-1)==e)return a.substring(1,a.length-1)}return a};
var za=function(a,b){var c=0,d=qa(String(a)).split("."),e=qa(String(b)).split("."),f=Math.max(d.length,e.length),h=0;for(;c==0&&h<f;h++){var j=d[h]||"",m=e[h]||"",l=new RegExp("(\\d*)(\\D*)","g"),A=new RegExp("(\\d*)(\\D*)","g");do{var v=l.exec(j)||["","",""],w=A.exec(m)||["","",""];if(v[0].length==0&&w[0].length==0)break;var s=v[1].length==0?0:parseInt(v[1],10),G=w[1].length==0?0:parseInt(w[1],10);c=ya(s,G)||ya(v[2].length==0,w[2].length==0)||ya(v[2],w[2])}while(c==0)}return c},ya=function(a,b){if(a<
b)return-1;else if(a>b)return 1;return 0};ja();var B,Aa,C,Ba,Ca,Da,Ea,Fa,Ga,Ha=function(){return u.navigator?u.navigator.userAgent:i};var Ia=function(){Da=Ca=Ba=C=Aa=B=g;var a;if(a=Ha()){var b=u.navigator;B=a.indexOf("Opera")==0;Aa=!B&&a.indexOf("MSIE")!=-1;Ba=(C=!B&&a.indexOf("WebKit")!=-1)&&a.indexOf("Mobile")!=-1;Da=(Ca=!B&&!C&&b.product=="Gecko")&&b.vendor=="Camino"}};Ia();
var D=B,E=Aa,F=Ca,H=C,Ja=Ba,Ka=function(){var a=u.navigator;return a&&a.platform||""},La=Ka(),Ma=function(){Ea=La.indexOf("Mac")!=-1;Fa=La.indexOf("Win")!=-1;Ga=La.indexOf("Linux")!=-1};Ma();var Na=Ea,Oa=Fa,Pa=Ga,Qa=function(){var a="",b;if(D&&u.opera){var c=u.opera.version;a=typeof c=="function"?c():c}else{if(F)b=/rv\:([^\);]+)(\)|;)/;else if(E)b=/MSIE\s+([^\);]+)(\)|;)/;else if(H)b=/WebKit\/(\S+)/;if(b){var d=b.exec(Ha());a=d?d[1]:""}}return a},Ra=Qa();
var Sa={},I=function(a){return Sa[a]||(Sa[a]=za(Ra,a)>=0)};var J;var Ta=function(a){return a?new K(L(a)):J||(J=new K)};
var Ua=function(a){return typeof a=="string"?document.getElementById(a):a},Va=Ua,Xa=function(a,b){pa(b,function(c,d){if(d=="style")a.style.cssText=c;else if(d=="class")a.className=c;else if(d=="for")a.htmlFor=c;else if(d in Wa)a.setAttribute(Wa[d],c);else a[d]=c})},Wa={cellpadding:"cellPadding",cellspacing:"cellSpacing",colspan:"colSpan",rowspan:"rowSpan",valign:"vAlign",height:"height",width:"width",usemap:"useMap",frameborder:"frameBorder",type:"type"},Ya=function(a){var b=a||u||window,c=b.document;
if(H&&!I("500")&&!Ja){if(typeof b.innerHeight=="undefined")b=window;var d=b.innerHeight,e=b.document.documentElement.scrollHeight;if(b==b.top)if(e<d)d-=15;return new z(b.innerWidth,d)}var f=Ta(c),h=f.e()&&(!D||D&&I("9.50"))?c.documentElement:c.body;return new z(h.clientWidth,h.clientHeight)},Za=function(){var a=J||(J=new K);return a.i.apply(a,arguments)},$a=function(a,b){a.appendChild(b)},bb=function(a){return a&&a.parentNode?a.parentNode.removeChild(a):i},eb=function(a,b){var c=b.parentNode;c&&c.replaceChild(a,
b)};
var fb=H&&za(Ra,"521")<=0,gb=function(a,b){if(typeof a.contains!="undefined"&&!fb&&b.nodeType==1)return a==b||a.contains(b);if(typeof a.compareDocumentPosition!="undefined")return a==b||Boolean(a.compareDocumentPosition(b)&16);for(;b&&a!=b;)b=b.parentNode;return b==a},L=function(a){return a.nodeType==9?a:a.ownerDocument||a.document},hb=function(a){if(a&&typeof a.length=="number")if(ha(a))return typeof a.item=="function"||typeof a.item=="string";else if(x(a)=="function")return typeof a.item=="function";return g},
K=function(a){this.d=a||u.document||document};
K.prototype.i=function(a,b){if(E&&b&&(b.name||b.type)){var c=["<",a];b.name&&c.push(' name="',wa(b.name),'"');if(b.type){c.push(' type="',wa(b.type),'"');b=ia(b);delete b.type}c.push(">");a=c.join("")}var d=this.createElement(a);b&&Xa(d,b);if(arguments.length>2){function e(j){if(j)this.appendChild(d,typeof j=="string"?this.createTextNode(j):j)}var f=2;for(;f<arguments.length;f++){var h=arguments[f];ga(h)&&!(ha(h)&&h.nodeType>0)?ka(hb(h)?la(h):h,e,this):e.call(this,h)}}return d};
K.prototype.createElement=function(a){return this.d.createElement(a)};K.prototype.createTextNode=function(a){return this.d.createTextNode(a)};K.prototype.e=function(){var a=this.d;if(a.compatMode)return a.compatMode=="CSS1Compat";if(H){var b=a.createElement("div");b.style.cssText="position:absolute;width:0;height:0;width:1";var c=b.style.width=="1px"?"BackCompat":"CSS1Compat";return(a.compatMode=c)=="CSS1Compat"}return g};
K.prototype.n=function(){var a=this.d;return!H&&this.e()?a.documentElement:a.body};K.prototype.m=function(){var a=this.n();return new y(a.scrollLeft,a.scrollTop)};K.prototype.appendChild=$a;K.prototype.removeNode=bb;K.prototype.replaceNode=eb;K.prototype.contains=gb;var ib,jb,kb,lb,mb,nb,ob=function(){nb=mb=lb=kb=jb=ib=g;var a=Ha();if(!!a)if(a.indexOf("Firefox")!=-1)ib=k;else if(a.indexOf("Camino")!=-1)jb=k;else if(a.indexOf("iPhone")!=-1||a.indexOf("iPod")!=-1)kb=k;else if(a.indexOf("Android")!=-1)lb=k;else if(a.indexOf("Chrome")!=-1)mb=k;else if(a.indexOf("Safari")!=-1)nb=k};ob();var pb=function(a,b){var c=L(a);if(c.defaultView&&c.defaultView.getComputedStyle){var d=c.defaultView.getComputedStyle(a,"");if(d)return d[b]}return i};var M=function(a,b){return pb(a,b)||(a.currentStyle?a.currentStyle[b]:i)||a.style[b]};
var qb=function(a){var b;b=a?a.nodeType==9?a:L(a):document;if(E&&!Ta(b).e())return b.body;return b.documentElement},rb=function(a){var b=a.getBoundingClientRect();if(E){var c=a.ownerDocument;b.left-=c.documentElement.clientLeft+c.body.clientLeft;b.top-=c.documentElement.clientTop+c.body.clientTop}return b},sb=function(a){if(E)return a.offsetParent;var b=L(a),c=M(a,"position"),d=c=="fixed"||c=="absolute",e=a.parentNode;for(;e&&e!=b;e=e.parentNode){c=M(e,"position");d=d&&c=="static"&&e!=b.documentElement&&
e!=b.body;if(!d&&(e.scrollWidth>e.clientWidth||e.scrollHeight>e.clientHeight||c=="fixed"||c=="absolute"))return e}return i},tb=function(a){var b,c=L(a),d=M(a,"position"),e=F&&c.getBoxObjectFor&&!a.getBoundingClientRect&&d=="absolute"&&(b=c.getBoxObjectFor(a))&&(b.screenX<0||b.screenY<0),f=new y(0,0),h=qb(c);if(a==h)return f;if(a.getBoundingClientRect){b=rb(a);var j=Ta(c).m();f.x=b.left+j.x;f.y=b.top+j.y}else if(c.getBoxObjectFor&&!e){b=c.getBoxObjectFor(a);var m=c.getBoxObjectFor(h);f.x=b.screenX-
m.screenX;f.y=b.screenY-m.screenY}else{var l=a;do{f.x+=l.offsetLeft;f.y+=l.offsetTop;if(l!=a){f.x+=l.clientLeft||0;f.y+=l.clientTop||0}if(H&&M(l,"position")=="fixed"){f.x+=c.body.scrollLeft;f.y+=c.body.scrollTop;break}l=l.offsetParent}while(l&&l!=a);if(D||H&&d=="absolute")f.y-=c.body.offsetTop;l=a;for(;(l=sb(l))&&l!=c.body;){f.x-=l.scrollLeft;if(!D||l.tagName!="TR")f.y-=l.scrollTop}}return f};F&&I("1.9");
var ub=function(a,b,c,d){if(/^\d+px?$/.test(b))return parseInt(b,10);else{var e=a.style[c],f=a.runtimeStyle[c];a.runtimeStyle[c]=a.currentStyle[c];a.style[c]=b;var h=a.style[d];a.style[c]=e;a.runtimeStyle[c]=f;return h}},vb=function(a){var b=L(a),c="";if(b.createTextRange){var d=b.body.createTextRange();d.moveToElementText(a);c=d.queryCommandValue("FontName")}if(!c){c=M(a,"fontFamily");if(D&&Pa)c=c.replace(/ \[[^\]]*\]/,"")}var e=c.split(",");if(e.length>1)c=e[0];return xa(c,"\"'")},wb=function(a){var b=
a.match(/[^\d]+$/);return b&&b[0]||i},xb={cm:1,"in":1,mm:1,pc:1,pt:1},yb={em:1,ex:1},zb=function(a){var b=M(a,"fontSize"),c=wb(b);if(b&&"px"==c)return parseInt(b,10);if(E)if(c in xb)return ub(a,b,"left","pixelLeft");else if(a.parentNode&&c in yb)return ub(a.parentNode,b,"left","pixelLeft");var d=Za("span",{style:"visibility:hidden;position:absolute;line-height:0;padding:0;margin:0;border:0;height:1em;"});$a(a,d);b=d.offsetHeight;bb(d);return b};var N=document,O=navigator,Q=window;
function Ab(){var a=N.cookie,b=Math.round((new Date).getTime()/1000),c=Q.google_analytics_domain_name,d=typeof c=="undefined"?Bb("auto"):Bb(c),e=a.indexOf("__utma="+d+".")>-1,f=a.indexOf("__utmb="+d)>-1,h=a.indexOf("__utmc="+d)>-1,j,m={};if(e){j=a.split("__utma="+d+".")[1].split(";")[0].split(".");m.sid=f&&h?j[3]+"":Q&&Q.gaGlobal&&Q.gaGlobal.sid?Q.gaGlobal.sid:b+"";m.vid=j[0]+"."+j[1];m.from_cookie=k}else{m.sid=Q&&Q.gaGlobal&&Q.gaGlobal.sid?Q.gaGlobal.sid:b+"";m.vid=Q&&Q.gaGlobal&&Q.gaGlobal.vid?
Q.gaGlobal.vid:(Cb()^Db()&2147483647)+"."+b;m.from_cookie=g}m.dh=d;m.hid=Q&&Q.gaGlobal&&Q.gaGlobal.hid?Q.gaGlobal.hid:Cb();return Q.gaGlobal=m}function Cb(){return Math.round(Math.random()*2147483647)}
function Db(){var a=N.cookie?N.cookie:"",b=Q.history.length,c,d,e=[O.appName,O.version,O.language?O.language:O.browserLanguage,O.platform,O.userAgent,O.javaEnabled()?1:0].join("");if(Q.screen)e+=Q.screen.width+"x"+Q.screen.height+Q.screen.colorDepth;else if(Q.java){d=java.awt.Toolkit.getDefaultToolkit().getScreenSize();e+=d.screen.width+"x"+d.screen.height}e+=a;e+=N.referrer?N.referrer:"";c=e.length;for(;b>0;)e+=b--^c++;return Eb(e)}
function Eb(a){var b=1,c=0,d,e;if(!(a==undefined||a=="")){b=0;d=a.length-1;for(;d>=0;d--){e=a.charCodeAt(d);b=(b<<6&268435455)+e+(e<<14);c=b&266338304;b=c!=0?b^c>>21:b}}return b}function Bb(a){if(!a||a==""||a=="none")return 1;if("auto"==a){a=N.domain;if("www."==a.substring(0,4))a=a.substring(4,a.length)}return Eb(a.toLowerCase())};var R="";function Fb(a){if(a){if(R!="")R+=",";R+=a}}var S=g,Gb=da("true",g);function Hb(a,b){var c="script";S=Ib(a,b);var d=!Jb();S&&d&&b.write("<"+c+' src="http://pagead2.googlesyndication.com/pagead/expansion_embed.js"></'+c+">");var e=Kb(a,b,r("1",0.01)),f=d||e;f&&fa()?b.write("<"+c+' src="http://pagead2.googlesyndication.com/pagead/render_ads.js"></'+c+">"):b.write("<"+c+">window.google_render_ad();</"+c+">")}
function T(a){return a!=i?'"'+a+'"':'""'}function q(a){return typeof encodeURIComponent=="function"?encodeURIComponent(a):escape(a)}function U(a,b){if(a&&b)window.google_ad_url+="&"+a+"="+b}function V(a){var b=window,c=o(a),d=b[a];U(c,d)}function W(a,b){b!=i&&U(a,q(b))}function X(a){var b=window,c=o(a),d=b[a];W(c,d)}function Y(a,b){var c=window,d=o(a),e=c[a];if(d&&e&&typeof e=="object")e=e[b%e.length];U(d,e)}
function Lb(a){var b=a.screen,c=navigator.javaEnabled(),d=-(new Date).getTimezoneOffset();if(b){U("u_h",b.height);U("u_w",b.width);U("u_ah",b.availHeight);U("u_aw",b.availWidth);U("u_cd",b.colorDepth)}U("u_tz",d);U("u_his",history.length);U("u_java",c);navigator.plugins&&U("u_nplug",navigator.plugins.length);navigator.mimeTypes&&U("u_nmime",navigator.mimeTypes.length)}
function Mb(a){if(!!a.google_enable_first_party_cookie){if(a._GA_googleCookieHelper==i)a._GA_googleCookieHelper=new p;if(!a._google_cookie_fetched){a._google_cookie_fetched=k;a._GA_googleCookieHelper.k(Nb(a.google_ad_client))}}}function Nb(a){if(a){a=a.toLowerCase();if(a.substring(0,3)!="ca-")a="ca-"+a}return a}function Ob(a){if(a){a=a.toLowerCase();if(a.substring(0,9)!="dist-aff-")a="dist-aff-"+a}return a}function Pb(a){var b="google_unique_id";if(a[b])++a[b];else a[b]=1;return a[b]}
function Qb(){var a=E&&I("6")&&!I("8"),b=F&&I("1.8.1"),c=H&&I("525");if(Oa&&(a||b||c))return k;else if(Na&&(c||b))return k;else if(Pa&&b)return k;return g}function Jb(){return typeof ExpandableAdSlotFactory=="function"&&typeof ExpandableAdSlotFactory.createIframe=="function"}function Ib(a,b){var c=a.google_allow_expandable_ads;if(c!=i&&c==g||!b.body||a.google_ad_output!="html"||Rb(a,b)||Sb(a)||Z(a.google_ad_format)||isNaN(a.google_ad_height)||isNaN(a.google_ad_width)||!Qb())return g;return k}
function Tb(){var a=Math.random(),b=r("0",0.03),c=2*b;if(a<b)return"30143019";if(a<c)return"30143020";return""}function Ub(){var a=Math.random(),b=r("0",0.03),c=2*b;if(a<b)return"30143021";if(a<c)return"30143022";return""}function Vb(){var a=Math.random(),b=r("0.01",0);if(a<b)return"68120011";if(a<2*b)return"68120021";if(a<3*b)return"68120031";if(a<4*b)return"68120041";return""}
function Wb(a){a.google_allow_expandable_ads=g;a.google_expandable_iframe=g}
function Xb(a,b,c,d){var e=Pb(a);c=c.substring(0,1992);c=c.replace(/%\w?$/,"");var f="script";if((a.google_ad_output=="js"||a.google_ad_output=="json_html")&&(a.google_ad_request_done||a.google_radlink_request_done))b.write("<"+f+' language="JavaScript1.1" src='+T($(c))+"></"+f+">");else if(a.google_ad_output=="html")if(Yb(a)&&Jb()){var h=a.google_container_id||d||i;a["google_expandable_ad_slot"+e]=ExpandableAdSlotFactory.createIframe("google_ads_frame"+e,$(c),a.google_ad_width,a.google_ad_height,
h)}else{var j='<iframe name="google_ads_frame" width='+T(a.google_ad_width)+" height="+T(a.google_ad_height)+" frameborder="+T(a.google_ad_frameborder)+" src="+T($(c))+' marginwidth="0" marginheight="0" vspace="0" hspace="0" allowtransparency="true" scrolling="no"></iframe>';j=Zb(a.google_ad_width,a.google_ad_height,j);a.google_container_id?$b(a.google_container_id,b,j):b.write(j)}else a.google_ad_output=="textlink"&&b.write("<"+f+' language="JavaScript1.1" src='+T($(c))+"></"+f+">")}
function Yb(a){if(!S)return g;var b=R.indexOf("30143020")!=-1,c=R.indexOf("30143019")!=-1,d=a.google_expandable_iframe;return Gb&&!c||b||d}function ac(a){var b=da("false",g);return b&&a.indexOf("30143021")==-1||a.indexOf("30143022")!=-1}
function Zb(a,b,c){var d=ac(R);if(S&&d){var e="border:none;height:"+b+"px;margin:0;padding:0;position:relative;visibility:visible;width:"+a+"px";return'<ins style="display:inline-table;'+e+'"><ins style="display:block;'+e+'">'+c+"</ins></ins>"}return c}function bc(a,b,c){if(!a)return g;if(!b)return k;return c}
function cc(a){for(var b in aa)a[b]=i;for(var b in ba)b=="google_correlator"||(a[b]=i);for(var b in ca)a[b]=i;a.google_allow_expandable_ads=i;a.google_container_id=i;a.google_expandable_iframe=i;a.google_tag_info=i}function Sb(a){if(a.google_ad_format)return a.google_ad_format.indexOf("_0ads")>0;return a.google_ad_output!="html"&&a.google_num_radlinks>0}function Z(a){return a&&a.indexOf("_sdo")!=-1}
function dc(a,b){var c=i,d=window,e=document,f=n,h=d.google_ad_format,j=ec(d),m;if(d.google_cpa_choice!=c){d.google_ad_url=j+"/cpa/ads?";m=escape(Nb(d.google_ad_client));d.google_ad_region="_google_cpa_region_";V("google_cpa_choice");if(typeof e.characterSet!="undefined")W("oe",e.characterSet);else typeof e.charset!="undefined"&&W("oe",e.charset)}else if(Z(h)){d.google_ad_url=j+"/pagead/sdo?";m=escape(Ob(d.google_ad_client))}else{d.google_ad_url=j+"/pagead/ads?";m=escape(Nb(d.google_ad_client))}d.google_ad_url+=
"client="+m;V("google_ad_host");V("google_ad_host_tier_id");var l=d.google_num_slots_by_client,A=d.google_num_slots_by_channel,v=d.google_prev_ad_formats_by_region,w=d.google_prev_ad_slotnames_by_region;if(d.google_ad_region==c&&d.google_ad_section!=c)d.google_ad_region=d.google_ad_section;var s=d.google_ad_region==c?"":d.google_ad_region;if(Z(h)){d.google_num_sdo_slots=d.google_num_sdo_slots?d.google_num_sdo_slots+1:1;if(d.google_num_sdo_slots>4)return g}else if(Sb(d)){d.google_num_0ad_slots=d.google_num_0ad_slots?
d.google_num_0ad_slots+1:1;if(d.google_num_0ad_slots>3)return g}else if(d.google_cpa_choice==c){d.google_num_ad_slots=d.google_num_ad_slots?d.google_num_ad_slots+1:1;if(d.google_num_slots_to_rotate){v[s]=c;w[s]=c;if(d.google_num_slot_to_show==c)d.google_num_slot_to_show=f%d.google_num_slots_to_rotate+1;if(d.google_num_slot_to_show!=d.google_num_ad_slots)return g}else if(d.google_num_ad_slots>6&&s=="")return g}U("dt",n);V("google_language");d.google_country?V("google_country"):V("google_gl");V("google_region");
X("google_city");X("google_hints");V("google_safe");V("google_encoding");V("google_last_modified_time");X("google_alternate_ad_url");V("google_alternate_color");V("google_skip");V("google_targeting");var G=d.google_ad_client;if(l[G])l[G]+=1;else{l[G]=1;l.length+=1}if(v[s])if(!Z(h)){W("prev_fmts",v[s].toLowerCase());l.length>1&&U("slot",l[G])}w[s]&&W("prev_slotnames",w[s].toLowerCase());if(bc(h,d.google_ad_slot,d.google_override_format)){W("format",h.toLowerCase());Z(h)||(v[s]=v[s]?v[s]+","+h:h)}else if(d.google_ad_slot)w[s]=
w[s]?w[s]+","+d.google_ad_slot:d.google_ad_slot;V("google_max_num_ads");U("output",d.google_ad_output);V("google_adtest");V("google_ad_callback");V("google_ad_slot");X("google_correlator");d.google_new_domain_checked==1&&d.google_new_domain_enabled==0&&U("dblk",1);if(d.google_ad_channel){X("google_ad_channel");var ab="",cb=d.google_ad_channel.split(fc),ma=0;for(;ma<cb.length;ma++){var na=cb[ma];if(A[na])ab+=na+"+";else A[na]=1}W("pv_ch",ab)}if(d.google_ad_host_channel){X("google_ad_host_channel");
var nc=gc(d.google_ad_host_channel,d.google_viewed_host_channels);W("pv_h_ch",nc)}d.google_enable_first_party_cookie&&W("cookie",d._GA_googleCookieHelper.o());X("google_page_url");Y("google_color_bg",f);Y("google_color_text",f);Y("google_color_link",f);Y("google_color_url",f);Y("google_color_border",f);Y("google_color_line",f);d.google_reuse_colors?U("reuse_colors",1):U("reuse_colors",0);V("google_font_face");V("google_kw_type");X("google_kw");X("google_contents");V("google_num_radlinks");V("google_max_radlink_len");
V("google_rl_filtering");V("google_rl_mode");V("google_rt");X("google_rl_dest_url");V("google_num_radlinks_per_unit");V("google_ad_type");V("google_image_size");V("google_ad_region");if(S)if(d.google_expandable_iframe===g)Wb(d);else if(m in{"ca-pub-2944451727872625":1,"ca-pub-9483266128490610":1,"ca-pub-1955924717845427":1,"ca-pub-6664249124335298":1})d.google_expandable_iframe=k;else{if(!(d.google_expandable_iframe==k)){var P=Tb();Fb(P);if(P=="30143019"||P==""&&!Gb)Wb(d);if(!P){P=Ub();Fb(P)}}}else Wb(d);
W("eid",R);var db=d.google_allow_expandable_ads;if(db!=i)db?U("ea","1"):U("ea","0");V("google_feedback");X("google_referrer_url");X("google_page_location");U("frm",d.google_iframing);V("google_bid");V("google_ctr_threshold");V("google_cust_age");V("google_cust_gender");V("google_cust_interests");V("google_cust_id");V("google_cust_job");V("google_cust_u_url");V("google_cust_l");V("google_cust_lh");V("google_cust_ch");V("google_ed");V("google_video_doc_id");V("google_video_product_type");X("google_ui_features");
X("google_ui_version");X("google_tag_info");X("google_only_ads_with_video");X("google_only_pyv_ads");X("google_disable_video_autoplay");if(a){W("ff",vb(a));W("fs",zb(a));var t;if(b)if(typeof a.getBoundingClientRect=="function"){t=a.getBoundingClientRect();t.x=t.left;t.y=t.top}else{t={};t.x="-252738";t.y="-252738"}else try{t=tb(a)}catch(Ac){t={};t.x="-252738";t.y="-252738"}var oa=Ya();if(t&&oa){W("biw",oa.width);W("bih",oa.height);W("adx",t.x);W("ady",t.y)}}Ab();U("ga_vid",d.gaGlobal.vid);U("ga_sid",
d.gaGlobal.sid);U("ga_hid",d.gaGlobal.hid);U("ga_fc",d.gaGlobal.from_cookie);X("google_analytics_uacct");V("google_ad_override");V("google_flash_version");Lb(d);return k}function gc(a,b){var c=a.split("|"),d=-1,e=[],f=0;for(;f<c.length;f++){var h=c[f].split(fc);b[f]||(b[f]={});var j="",m=0;for(;m<h.length;m++){var l=h[m];if(!(l==""))if(b[f][l])j+="+"+l;else b[f][l]=1}j=j.slice(1);e[f]=j;if(j!="")d=f}var A="";if(d>-1){var f=0;for(;f<d;f++)A+=e[f]+"|";A+=e[d]}return A}
function hc(){var a=window,b=document;Mb(a);var c=Vb();Fb(c);var d,e=g,f=g,h=g;switch(c){case "68120031":h=k;case "68120021":f=k;case "68120041":e=k}if(e){var j="google_temp_span";d=a.google_container_id&&Va(a.google_container_id)||Va(j);if(!d&&!a.google_container_id){b.write("<span id="+j+"></span>");d=Va(j)}}var m=dc(f&&d,h);d&&d.id==j&&bb(d);if(!!m){Xb(a,b,a.google_ad_url);cc(a)}}function $(a){var b=(new Date).getTime()-n,c="&dtd="+(b<1000?b:"M");return a+c}function ic(){hc();return k}
function Rb(a,b){if(a.top.location==b.location)return g;var c=b.documentElement;if(a.google_ad_width&&a.google_ad_height){var d=1,e=1;if(a.innerHeight){d=a.innerWidth;e=a.innerHeight}else if(c&&c.clientHeight){d=c.clientWidth;e=c.clientHeight}else if(b.body){d=b.body.clientWidth;e=b.body.clientHeight}if(e>2*a.google_ad_height||d>2*a.google_ad_width)return g}return k}
function jc(a){var b=window,c=i,d=b.onerror;b.onerror=a;if(b.google_ad_frameborder==c)b.google_ad_frameborder=0;if(b.google_ad_output==c)b.google_ad_output="html";if(Z(b.google_ad_format)){var e=b.google_ad_format.match(/^(\d+)x(\d+)_.*/);if(e){b.google_ad_width=parseInt(e[1],10);b.google_ad_height=parseInt(e[2],10);b.google_ad_output="html"}}if(b.google_ad_format==c&&b.google_ad_output=="html")b.google_ad_format=b.google_ad_width+"x"+b.google_ad_height;kc(b,document);if(b.google_num_slots_by_channel==
c)b.google_num_slots_by_channel=[];if(b.google_viewed_host_channels==c)b.google_viewed_host_channels=[];if(b.google_num_slots_by_client==c)b.google_num_slots_by_client=[];if(b.google_prev_ad_formats_by_region==c)b.google_prev_ad_formats_by_region=[];if(b.google_prev_ad_slotnames_by_region==c)b.google_prev_ad_slotnames_by_region=[];if(b.google_correlator==c)b.google_correlator=n;if(b.google_adslot_loaded==c)b.google_adslot_loaded={};if(b.google_adContentsBySlot==c)b.google_adContentsBySlot={};if(b.google_flash_version==
c)b.google_flash_version=ea();if(b.google_new_domain_checked==c)b.google_new_domain_checked=0;if(b.google_new_domain_enabled==c)b.google_new_domain_enabled=0;b.onerror=d}function lc(a){if(a in mc)return mc[a];return mc[a]=navigator.userAgent.toLowerCase().indexOf(a)!=-1}var mc={};function fa(){return lc("msie")&&!window.opera}
function oc(a){var b={},c=a.split("?"),d=c[c.length-1].split("&"),e=0;for(;e<d.length;e++){var f=d[e].split("=");if(f[0])try{b[f[0].toLowerCase()]=f.length>1?window.decodeURIComponent?decodeURIComponent(f[1].replace(/\+/g," ")):unescape(f[1]):""}catch(h){}}return b}function pc(){var a=window,b=oc(document.URL);if(b.google_ad_override){a.google_ad_override=b.google_ad_override;a.google_adtest="on"}}function qc(a,b){for(var c in b)a["google_"+c]=b[c]}
function rc(a,b){if(!b)return a.location;return a.referrer}function sc(a,b){if(!b&&a.google_referrer_url==i)return"0";else if(b&&a.google_referrer_url==i)return"1";else if(!b&&a.google_referrer_url!=i)return"2";else if(b&&a.google_referrer_url!=i)return"3";return"4"}function tc(a,b,c,d){a.page_url=rc(c,d);a.page_location=i}function uc(a,b,c,d){a.page_url=b.google_page_url;a.page_location=rc(c,d)||"EMPTY"}
function vc(a,b){var c={},d=Rb(a,b);c.iframing=sc(a,d);!!a.google_page_url?uc(c,a,b,d):tc(c,a,b,d);c.last_modified_time=b.location==c.page_url?Date.parse(b.lastModified)/1000:i;c.referrer_url=d?a.google_referrer_url:a.google_page_url&&a.google_referrer_url?a.google_referrer_url:b.referrer;return c}function wc(a){var b={},c=a.URL.substring(a.URL.lastIndexOf("http"));b.iframing=i;b.page_url=c;b.page_location=a.location;b.last_modified_time=i;b.referrer_url=c;return b}
function kc(a,b){var c;c=a.google_page_url==i&&xc[b.domain]?wc(b):vc(a,b);qc(a,c)}function $b(a,b,c){if(a){var d=b.getElementById(a);if(d&&c&&c.length!=""){d.style.visibility="visible";d.innerHTML=c}}}var xc={};xc["ad.yieldmanager.com"]=k;var fc=/[+, ]/;window.google_render_ad=hc;var yc={google:1,googlegroups:1,gmail:1,googlemail:1,googleimages:1,googleprint:1};function zc(a){var b=a.google_page_location||a.google_page_url;if(!b)return g;b=b.toString();if(b.indexOf("http://")==0)b=b.substring(7,b.length);else if(b.indexOf("https://")==0)b=b.substring(8,b.length);var c=b.indexOf("/");if(c==-1)c=b.length;var d=b.substring(0,c),e=d.split("."),f=g;if(e.length>=3)f=e[e.length-3]in yc;if(e.length>=2)f=f||e[e.length-2]in yc;return f}
function Kb(a,b,c){if(zc(a)){a.google_new_domain_checked=1;return g}if(a.google_new_domain_checked==0){var d=Math.random();if(d<=c){var e="http://googleads.g.doubleclick.net/pagead/test_domain.js",f="script";b.write("<"+f+' src="'+e+'"></'+f+">");a.google_new_domain_checked=1;return k}}return g}function ec(a){var b="http://googleads.g.doubleclick.net",c="http://pagead2.googlesyndication.com";if(!zc(a)&&a.google_new_domain_enabled==1)return b;return c};pc();jc(ic);Hb(window,document);
})()