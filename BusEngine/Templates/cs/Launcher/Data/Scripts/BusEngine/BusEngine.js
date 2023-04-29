/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
'use strict';
'use asm';
/* Chrome error off/block cookie "'Window': Access is denied for this document." */
try {
	window.localStorage.length;
	window.localStorage.status = true;
	window.sessionStorage.length;
	window.sessionStorage.status = true;
} catch (e) {
	delete window.localStorage;
	delete window.sessionStorage;
	window.localStorage = {
		'status':false,
		'getItem':function(a) {},
		'setItem':function(a, b) {},
		'key':function(a) {},
		'removeItem':function(a) {},
		'clear':function() {},
		'length':0
	};
	window.sessionStorage = window.localStorage;
} finally {

}

/* window.addEventListener('load', function(e) {
	if (!window.localStorage.status && !window.sessionStorage.status && !document.cookie) {
		setTimeout(function() {
			window.alert('Каб выкарыстоўваць усе магчымасці сайта, неабходна ўключыць cookie.');
		}, 1000);
	}
}); */

// Fix Internet Explorer
if (!window.console) {
	window.console = {};
	window.console.log = window.console.assert = function(){};
	window.console.warn = window.console.assert = function(){};
}

// contextmenu off Internet Explorer 8+
/* document.oncontextmenu = function () {
	return false;
}; */

if (!('BusEngine' in window)) {
	window.BusEngine = {};
}
if (!('PostMessage' in window.BusEngine)) {
	window.BusEngine.PostMessage = function(m) {};
}

window.console.logs = window.console.log;
BusEngine.log = window.console.log = function(...args) {
	var l = new Error().stack.split('\n');
	if ('length' in l && l.length > 0) {
		l = l[l.length-1].match(/(?<=\().*?(?=\))/);
		if (l) {
			args.push(l[0]);
		}
	}
	window.console.logs.apply(this, args);
};

if (!('Localization' in window.BusEngine)) {
	BusEngine.Localization = {};
}

BusEngine.Localization.Initialize = function() {
	var i, l, localization = document.querySelectorAll('[data-localization]');
	l = localization.length;

	for (i = 0; i < l; ++i) {
		localization[i];
	}
};
if (!('GetLanguages' in window.BusEngine.Localization)) {
	BusEngine.Localization.GetLanguages = {};
}
BusEngine.Localization.get = function(key) {
	if (Object.hasOwn(BusEngine.Localization.GetLanguages, key)) {
		return BusEngine.Localization.GetLanguages[key];
	} else {
		return key;
	}
};
BusEngine.Localization.set = function(key, value) {
	BusEngine.Localization.GetLanguages[key] = value;
};

// https://developer.mozilla.org/ru/docs/Web/API/HTMLMediaElement
BusEngine.polyfillTagSource = function(ex) {
	if (typeof ex == 'undefined') {
		ex = [];
	}
	var i, l, v = document.querySelectorAll('video source[media]:not([data-error])');
	l = v.length;

	for (i = 0; i < l; ++i) {
		if (window.matchMedia(v[i].media).matches) {
			if (v[i].getAttribute('data-src') && ex.indexOf(v[i].getAttribute('data-src')) == -1) {
				v[i].setAttribute('src', v[i].getAttribute('data-src'));
				v[i].removeAttribute('data-src');
				v[i].parentNode.addEventListener('error', function(e) {
					e.target.setAttribute('data-error', e.target.src);
					ex.push(e.target.src);
					BusEngine.polyfillTagSource(ex);
				});
				v[i].parentNode.src = v[i].getAttribute('src');
				break;
			}
		} else {
			if (v[i].getAttribute('src')) {
				v[i].setAttribute('data-src', v[i].getAttribute('src'));
				v[i].removeAttribute('src');
			}
		}
	}
};

BusEngine.cookie = {
	'set': function(name, value, domain, path, day) {
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		if (typeof value == 'undefined' || typeof value != 'string') {
			value = '';
		}

		if (typeof domain == 'object' && domain != null) {
			console.log(domain);
			if ('path' in domain) {
				path = domain.path;
			}
			if ('day' in domain) {
				day = domain.day;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
			path = '/';
		}

		if (typeof day != 'undefined' && 'Date' in window) {
			var x = new window.Date();
			x.setUTCSeconds(3600 * 24 * Number(day));
			day = ' expires=' + x + ';';
		} else {
			day = '';
		}

		document.cookie = name + '=' + value + '; path=' + path + ';' + day + ' domain=' + domain;

		return true;
	},
	'get': function(name) {
		var c = document.cookie;

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return c;
		}

		//console.log(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));
		//console.log(/(name)\=(\S[^\;]+)/);

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (c && c[2]) {
			return c[2];
		} else {
			return '';
		}
	},
	'remove': function(name, value, domain, path) {
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		var v;

		if (typeof value == 'undefined' || typeof value != 'string') {
			v = '';
		} else {
			v = value;
		}

		if (typeof domain == 'object' && domain != null) {
			if ('path' in domain) {
				path = domain.path;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
			path = '/';
		}

		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=';
		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=' + domain;

		return true;
	},
	'has': function(name, value) {
		var c = document.cookie;

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (typeof value == 'undefined' || typeof value != 'string') {
			if (c && c[1] && c[1] == name) {
				return true;
			}
		} else {
			if (c && c[2] && c[2] == value) {
				return true;
			}
		}

		return false;
	},
	'test': function() {
		// добавить
		BusEngine.cookie.set('BusEngine', 'Like');
		console.log(document.cookie);

		// получить
		console.log(BusEngine.cookie.get('BusEngine'));

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));

		// удалить
		BusEngine.cookie.remove('BusEngine');
		console.log(document.cookie);

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));
	}
};

BusEngine.loadScript = function(url, callback) {
	var s, ss;
	s = document.createElement('script');
	s.type = 'text/javascript';
	s.src = url;
	if (typeof callback !== 'undefined') {
		s.onreadystatechange = callback;
		s.onload = callback;
	}
	ss = document.head;
	if (ss) {
		ss.appendChild(s);
	}
};