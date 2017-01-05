var psEPyQsid="qzfWExH2Rh15";
// safe-textlink@gecko.js

var psEPyQiso;
try {
	psEPyQiso = (opener != null) && (typeof(opener.name) != "unknown") && (opener.psEPyQwid != null);
} catch(e) {
	psEPyQiso = false;
}
if (psEPyQiso) {
	window.psEPyQwid = opener.psEPyQwid + 1;
	psEPyQsid = psEPyQsid + "_" + window.psEPyQwid;
} else {
	window.psEPyQwid = 1;
}
function psEPyQn() {
	return (new Date()).getTime();
}
var psEPyQs = psEPyQn();
function psEPyQst(f, t) {
	if ((psEPyQn() - psEPyQs) < 7200000) {
		return setTimeout(f, t * 1000);
	} else {
		return null;
	}
}
var psEPyQol = true;
function psEPyQow() {
	if (psEPyQol || (1 == 1)) {
		var pswo = "menubar=0,location=0,scrollbars=auto,resizable=1,status=0,width=650,height=680";
		var pswn = "pscw_" + psEPyQn();
		var url = "http://messenger.providesupport.com/messenger/1crx3r4qv22at146c42g4aefy8.html?ps_l=" + escape(document.location) + "";
		window.open(url, pswn, pswo);
	} else if (1 == 2) {
		document.location = "http://";
	}
}
var psEPyQil;
var psEPyQit;
function psEPyQpi() {
	var il;
	if (3 == 2) {
		il = window.pageXOffset + 50;
	} else if (3 == 3) {
		il = (window.innerWidth * 50 / 100) + window.pageXOffset;
	} else {
		il = 50;
	}
	il -= (271 / 2);
	var it;
	if (3 == 2) {
		it = window.pageYOffset + 50;
	} else if (3 == 3) {
		it = (window.innerHeight * 50 / 100) + window.pageYOffset;
	} else {
		it = 50;
	}
	it -= (191 / 2);
	if ((il != psEPyQil) || (it != psEPyQit)) {
		psEPyQil = il;
		psEPyQit = it;
		var d = document.getElementById('ciEPyQ');
		if (d != null) {
			d.style.left  = Math.round(psEPyQil) + "px";
			d.style.top  = Math.round(psEPyQit) + "px";
		}
	}
	setTimeout("psEPyQpi()", 100);
}
var psEPyQlc = 0;
function psEPyQsi(t) {
	window.onscroll = psEPyQpi;
	window.onresize = psEPyQpi;
	psEPyQpi();
	psEPyQlc = 0;
	var url = "http://messenger.providesupport.com/" + ((t == 2) ? "auto" : "chat") + "-invitation/1crx3r4qv22at146c42g4aefy8.html?ps_t=" + psEPyQn() + "&ps_vsid=" + psEPyQsid + "";
	var d = document.getElementById('ciEPyQ');
	if (d != null) {
		d.innerHTML = '<iframe allowtransparency="true" style="background:transparent;width:271;height:191" src="' + url + 
			'" onload="psEPyQld()" frameborder="no" width="271" height="191" scrolling="no"></iframe>';
	}
}
function psEPyQld() {
	if (psEPyQlc == 1) {
		var d = document.getElementById('ciEPyQ');
		if (d != null) {
			d.innerHTML = "";
		}
	}
	psEPyQlc++;
}
if (false) {
	psEPyQsi(1);
}
var psEPyQd = document.getElementById('scEPyQ');
if (psEPyQd != null) {
	if (psEPyQol || (1 == 1) || (1 == 2)) {
		var ctt = "";
		if (ctt != "") {
			tt = ' alt="' + ctt + '" title="' + ctt + '"';
		} else {
			tt = '';
		}
		if (false) {
			psEPyQd.innerHTML = '<table style="display:inline;border:0px;border-collapse:collapse;border-spacing:0;"><tr><td style="padding:0px;text-align:center;border:0px"><a href="#" onclick="psEPyQow(); return false;"'+tt+'><span id="psEPyQl">' + 'Live Chat Online' + '</span></a></td></tr><tr><td style="padding:0px;text-align:center;border:0px"><a href="http://www.providesupport.com/pb/1crx3r4qv22at146c42g4aefy8" target="_blank"><img src="http://image.providesupport.com/lcbpsh.gif" width="140" height="17" style="border:0;display:block;margin:auto"></a></td></tr></table>';
		} else {
			psEPyQd.innerHTML = '<a href="#" onclick="psEPyQow(); return false;"'+tt+'><span id="psEPyQl">' + 'Live Chat Online' + '</span></a>';
		}
	} else {
		psEPyQd.innerHTML = '';
	}
}
var psEPyQop = false;
function psEPyQco() {
	var w1 = psEPyQci.width - 1;
	psEPyQol = (w1 & 1) != 0;
	psEPyQsl(psEPyQol ? "Live Chat Online" : "Live Chat Offline");
	psEPyQscf((w1 & 2) != 0);
	var h = psEPyQci.height;

	if (h == 1) {
		psEPyQop = false;
	
	} else if ((h == 2) && (!psEPyQop)) {
		psEPyQop = true;
		psEPyQsi(1);

	} else if ((h == 3) && (!psEPyQop)) {
		psEPyQop = true;
		psEPyQsi(2);
	}
}
var psEPyQci = new Image();
psEPyQci.onload = psEPyQco;
var psEPyQpm = false;
var psEPyQcp = psEPyQpm ? 30 : 60;
var psEPyQct = null;
function psEPyQscf(p) {
	if (psEPyQpm != p) {
		psEPyQpm = p;
		psEPyQcp = psEPyQpm ? 30 : 60;
		if (psEPyQct != null) {
			clearTimeout(psEPyQct);
			psEPyQct = null;
		}
		psEPyQct = psEPyQst("psEPyQrc()", psEPyQcp);
	}
}
function psEPyQrc() {
	psEPyQct = psEPyQst("psEPyQrc()", psEPyQcp);
	try {
		psEPyQci.src = "http://image.providesupport.com/cmd/1crx3r4qv22at146c42g4aefy8?" + "ps_t=" + psEPyQn() + "&ps_l=" + escape(document.location) + "&ps_r=" + escape(document.referrer) + "&ps_s=" + psEPyQsid + "" + "";
	} catch(e) {
	}
}
psEPyQrc();
var psEPyQcl = "Live Chat Online";
function psEPyQsl(l) {
	if (psEPyQcl != l) {
		var d = document.getElementById('psEPyQl');
		if (d != null) {
			d.innerHTML = l;
		}
		psEPyQcl = l;
	}
}

