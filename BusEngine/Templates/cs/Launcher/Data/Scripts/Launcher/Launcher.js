/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
'use strict';
'use asm';
	function svgToggle(e, type) {
		if (typeof type == 'undefined') {
			type = '';
		}

		if (type == 'password') {
			var p = e.parentNode.querySelector('input');
			
			if (p) {
				if (p.getAttribute('type') == 'password') {
					p.setAttribute('type', 'text');
				} else {
					p.setAttribute('type', 'password');
				}
			}
		}

		var i, elements = e.querySelectorAll('svg');

		if (elements) {
			for (i = 0; i < elements.length; ++i) {
				if (elements[i].style['display'] == 'none') {
					elements[i].style['display'] = '';
				} else {
					elements[i].style['display'] = 'none';
				}
			}
		}
	}

	window.addEventListener('DOMContentLoaded', function() {
		// изменение размера окна мышкой
		var fix, nx, ny, cursor, x, y, my_bstatus, my_status, w, h, my_x = document.querySelector('.x'), my_y = document.querySelector('.y');

		document.addEventListener('mousedown', function(e) {
			cursor = 0;
			x = e.clientX;
			y = e.clientY;
			fix = 0;
			if (e.which == 1 && my_status) {
				my_bstatus = true;
			}
		});

		document.addEventListener('mouseout', function(e) {
			if (e.which != 1) {
				document.body.style['cursor'] = 'default';
				my_bstatus = false;
				cursor = 0;
			}
		});
 
		document.addEventListener('mousemove', function(e) {
			// https://habr.com/ru/articles/509258/
			/* console.log('экран All', [window.screen.width, window.screen.height]);
			console.log('экран ', [window.screen.availWidth, window.screen.availHeight]);
			console.log('окно приложения All ', [window.outerWidth, window.outerHeight]);
			console.log('окно приложения ', [window.innerWidth, window.innerHeight]); */
			w = window.innerWidth - 10;
			h = window.innerHeight - 10;
			my_status = false;

			if (e.clientX <= 10) {
				// https://developer.mozilla.org/en-US/docs/Web/CSS/cursor
				if (cursor == 0) {
					document.body.style['cursor'] = 'ew-resize'; // влево
					cursor = 1;
				}
				my_status = true;
			}

			if (e.clientY <= 10) {
				if (cursor == 0) {
					document.body.style['cursor'] = 'ns-resize'; // вверх
					cursor = 3;
				}
				my_status = true;
			}
			if (e.clientX >= w) {
				if (cursor == 0) {
					document.body.style['cursor'] = 'ew-resize'; // вправо
					cursor = 5;
				}
				my_status = true;
			}
			if (e.clientY >= h) {
				if (cursor == 0) {
					document.body.style['cursor'] = 'ns-resize'; // вниз
					cursor = 7;
				}
				my_status = true;
			}
			if (e.clientX <= 10 && e.clientY <= 10) {
				if (cursor == 0 || cursor == 1 || cursor == 3) {
					document.body.style['cursor'] = 'nwse-resize'; // левый-верхний
					cursor = 2;
				}
				my_status = true;
			}
			if (e.clientX >= w && e.clientY <= 10) {
				if (cursor == 0 || cursor == 5 || cursor == 3) {
					document.body.style['cursor'] = 'nesw-resize'; // правый-верхний
					cursor = 4;
				}
				my_status = true;
			}
			if (e.clientX >= w && e.clientY >= h) {
				if (cursor == 0 || cursor == 5 || cursor == 7) {
					document.body.style['cursor'] = 'nwse-resize'; // правый-нижний
					cursor = 6;
				}
				my_status = true;
			}
			if (e.clientX <= 10 && e.clientY >= h) {
				if (cursor == 0 || cursor == 1 || cursor == 7) {
					document.body.style['cursor'] = 'nesw-resize'; // левый-нижний
					cursor = 8;
				}
				my_status = true;
			}

			if (e.which == 1 && my_bstatus) {
				if (cursor == 1) {
					nx = (e.clientX - x);
					ny = 0;
				} else if (cursor == 2) {
					nx = (e.clientX - x);
					ny = (e.clientY - y);
				} else if (cursor == 3) {
					nx = 0;
					ny = (e.clientY - y);
				} else if (cursor == 4) {
					nx = (e.clientX - window.innerWidth+1);
					ny = (e.clientY - y);
				} else if (cursor == 5) {
					nx = (e.clientX - window.innerWidth+1);
					ny = 0;
				} else if (cursor == 6) {
					nx = (e.clientX - window.innerWidth+1);
					ny = (e.clientY - window.innerHeight+1);
				} else if (cursor == 7) {
					nx = 0;
					ny = (e.clientY - window.innerHeight+1);
				} else if (cursor == 8) {
					nx = (e.clientX - x);
					ny = (e.clientY - window.innerHeight+1);
				}
				fix++;
				BusEngine.PostMessage('_resize|' + nx + ' ' + ny + ' ' + cursor + ' ' + fix);

				//BusEngine.log(nx, ny, cursor, fix);
				my_x.innerHTML = e.offsetX + ' ' + w;
				my_y.innerHTML = e.offsetY + ' ' + h;
			}

			if (e.which != 1 && !my_status) {
				document.body.style['cursor'] = 'default';
				my_bstatus = false;
				cursor = 0;
			}
		});

		// перемещение окна приложения
		var point = document.querySelector('#top .point');

		if (point) {
			var x, y, p = function(e) {
				if (e.which == 1) {
					//BusEngine.log((e.clientX - x), (e.clientY - y));
					BusEngine.PostMessage('__point|' + (e.clientX - x) + ' ' + (e.clientY - y));
				}
			};

			point.addEventListener('mousedown', function(e) {
				x = e.clientX;
				y = e.clientY;
				//BusEngine.log(x, y);
				e.target.style['cursor'] = 'grabbing';
				document.addEventListener('mousemove', p);
			});

			point.addEventListener('mouseup', function(e) {
				document.removeEventListener('mousemove', p);
				e.target.style['cursor'] = '';
			});
		}
	});