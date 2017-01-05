/*
 Highstock JS v2.0.1 (2014-04-24)

 (c) 2009-2014 Torstein Honsi

 License: www.highcharts.com/license
*/
(function() {
    function v(a, b) {
        var c;
        a || (a = {});
        for (c in b) a[c] = b[c];
        return a
    }
    function y() {
        var a, b = arguments,
        c, d = {},
        e = function(a, b) {
            var c, d;
            typeof a !== "object" && (a = {});
            for (d in b) b.hasOwnProperty(d) && (c = b[d], a[d] = c && typeof c === "object" && Object.prototype.toString.call(c) !== "[object Array]" && d !== "renderTo" && typeof c.nodeType !== "number" ? e(a[d] || {},
            c) : b[d]);
            return a
        };
        b[0] === !0 && (d = b[1], b = Array.prototype.slice.call(b, 2));
        c = b.length;
        for (a = 0; a < c; a++) d = e(d, b[a]);
        return d
    }
    function Kb() {
        for (var a = 0,
        b = arguments,
        c = b.length,
        d = {}; a < c; a++) d[b[a++]] = b[a];
        return d
    }
    function I(a, b) {
        return parseInt(a, b || 10)
    }
    function Oa(a) {
        return typeof a === "string"
    }
    function fa(a) {
        return typeof a === "object"
    }
    function Pa(a) {
        return Object.prototype.toString.call(a) === "[object Array]"
    }
    function la(a) {
        return typeof a === "number"
    }
    function Ga(a) {
        return V.log(a) / V.LN10
    }
    function oa(a) {
        return V.pow(10, a)
    }
    function pa(a, b) {
        for (var c = a.length; c--;) if (a[c] === b) {
            a.splice(c, 1);
            break
        }
    }
    function t(a) {
        return a !== r && a !== null
    }
    function W(a, b, c) {
        var d, e;
        if (Oa(b)) t(c) ? a.setAttribute(b, c) : a && a.getAttribute && (e = a.getAttribute(b));
        else if (t(b) && fa(b)) for (d in b) a.setAttribute(d, b[d]);
        return e
    }
    function ma(a) {
        return Pa(a) ? a: [a]
    }
    function o() {
        var a = arguments,
        b, c, d = a.length;
        for (b = 0; b < d; b++) if (c = a[b], typeof c !== "undefined" && c !== null) return c
    }
    function E(a, b) {
        if (Ha && !ca && b && b.opacity !== r) b.filter = "alpha(opacity=" + b.opacity * 100 + ")";
        v(a.style, b)
    }
    function $(a, b, c, d, e) {
        a = B.createElement(a);
        b && v(a, b);
        e && E(a, {
            padding: 0,
            border: Y,
            margin: 0
        });
        c && E(a, c);
        d && d.appendChild(a);
        return a
    }
    function ga(a, b) {
        var c = function() {};
        c.prototype = new a;
        v(c.prototype, b);
        return c
    }
    function Ia(a, b, c, d) {
        var e = F.lang,
        a = +a || 0,
        f = b === -1 ? (a.toString().split(".")[1] || "").length: isNaN(b = M(b)) ? 2 : b,
        b = c === void 0 ? e.decimalPoint: c,
        d = d === void 0 ? e.thousandsSep: d,
        e = a < 0 ? "-": "",
        c = String(I(a = M(a).toFixed(f))),
        g = c.length > 3 ? c.length % 3 : 0;
        return e + (g ? c.substr(0, g) + d: "") + c.substr(g).replace(/(\d{3})(?=\d)/g, "$1" + d) + (f ? b + M(a - c).toFixed(f).slice(2) : "")
    }
    function Qa(a, b) {
        return Array((b || 2) + 1 - String(a).length).join(0) + a
    }
    function O(a, b, c) {
        var d = a[b];
        a[b] = function() {
            var a = Array.prototype.slice.call(arguments);
            a.unshift(d);
            return c.apply(this, a)
        }
    }
    function Ja(a, b) {
        for (var c = "{",
        d = !1,
        e, f, g, h, i, k = []; (c = a.indexOf(c)) !== -1;) {
            e = a.slice(0, c);
            if (d) {
                f = e.split(":");
                g = f.shift().split(".");
                i = g.length;
                e = b;
                for (h = 0; h < i; h++) e = e[g[h]];
                if (f.length) f = f.join(":"),
                g = /\.([0-9])/,
                h = F.lang,
                i = void 0,
                /f$/.test(f) ? (i = (i = f.match(g)) ? i[1] : -1, e !== null && (e = Ia(e, i, h.decimalPoint, f.indexOf(",") > -1 ? h.thousandsSep: ""))) : e = ua(f, e)
            }
            k.push(e);
            a = a.slice(c + 1);
            c = (d = !d) ? "}": "{"
        }
        k.push(a);
        return k.join("")
    }
    function rb(a) {
        return V.pow(10, S(V.log(a) / V.LN10))
    }
    function sb(a, b, c, d) {
        var e, c = o(c, 1);
        e = a / c;
        b || (b = [1, 2, 2.5, 5, 10], d && d.allowDecimals === !1 && (c === 1 ? b = [1, 2, 5, 10] : c <= 0.1 && (b = [1 / c])));
        for (d = 0; d < b.length; d++) if (a = b[d], e <= (b[d] + (b[d + 1] || b[d])) / 2) break;
        a *= c;
        return a
    }
    function Lb() {
        this.symbol = this.color = 0
    }
    function tb(a, b) {
        var c = a.length,
        d, e;
        for (e = 0; e < c; e++) a[e].ss_i = e;
        a.sort(function(a, c) {
            d = b(a, c);
            return d === 0 ? a.ss_i - c.ss_i: d
        });
        for (e = 0; e < c; e++) delete a[e].ss_i
    }
    function Ra(a) {
        for (var b = a.length,
        c = a[0]; b--;) a[b] < c && (c = a[b]);
        return c
    }
    function Ba(a) {
        for (var b = a.length,
        c = a[0]; b--;) a[b] > c && (c = a[b]);
        return c
    }
    function Ka(a, b) {
        for (var c in a) a[c] && a[c] !== b && a[c].destroy && a[c].destroy(),
        delete a[c]
    }
    function Sa(a) {
        hb || (hb = $(Ta));
        a && hb.appendChild(a);
        hb.innerHTML = ""
    }
    function qa(a, b) {
        var c = "Highcharts error #" + a + ": www.highcharts.com/errors/" + a;
        if (b) throw c;
        else X.console && console.log(c)
    }
    function ha(a) {
        return parseFloat(a.toPrecision(14))
    }
    function Ya(a, b) {
        Ca = o(a, b.animation)
    }
    function Mb() {
        var a = F.global.useUTC,
        b = a ? "getUTC": "get",
        c = a ? "setUTC": "set";
        La = (a && F.global.timezoneOffset || 0) * 6E4;
        ib = a ? Date.UTC: function(a, b, c, g, h, i) {
            return (new Date(a, b, o(c, 1), o(g, 0), o(h, 0), o(i, 0))).getTime()
        };
        ub = b + "Minutes";
        vb = b + "Hours";
        wb = b + "Day";
        Ua = b + "Date";
        jb = b + "Month";
        kb = b + "FullYear";
        Nb = c + "Minutes";
        Ob = c + "Hours";
        xb = c + "Date";
        Pb = c + "Month";
        Qb = c + "FullYear"
    }
    function Z() {}
    function Za(a, b, c, d) {
        this.axis = a;
        this.pos = b;
        this.type = c || "";
        this.isNew = !0; ! c && !d && this.addLabel()
    }
    function L() {
        this.init.apply(this, arguments)
    }
    function va() {
        this.init.apply(this, arguments)
    }
    function Rb(a, b, c, d, e) {
        var f = a.chart.inverted;
        this.axis = a;
        this.isNegative = c;
        this.options = b;
        this.x = d;
        this.total = null;
        this.points = {};
        this.stack = e;
        this.alignOptions = {
            align: b.align || (f ? c ? "left": "right": "center"),
            verticalAlign: b.verticalAlign || (f ? "middle": c ? "bottom": "top"),
            y: o(b.y, f ? 4 : c ? 14 : -6),
            x: o(b.x, f ? c ? -6 : 6 : 0)
        };
        this.textAlign = b.textAlign || (f ? c ? "right": "left": "center")
    }
    function yb(a) {
        var b = a.options,
        c = b.navigator,
        d = c.enabled,
        b = b.scrollbar,
        e = b.enabled,
        f = d ? c.height: 0,
        g = e ? b.height: 0;
        this.handles = [];
        this.scrollbarButtons = [];
        this.elementsToDestroy = [];
        this.chart = a;
        this.setBaseSeries();
        this.height = f;
        this.scrollbarHeight = g;
        this.scrollbarEnabled = e;
        this.navigatorEnabled = d;
        this.navigatorOptions = c;
        this.scrollbarOptions = b;
        this.outlineHeight = f + g;
        this.init()
    }
    function zb(a) {
        this.init(a)
    }
    var r, B = document,
    X = window,
    V = Math,
    s = V.round,
    S = V.floor,
    Va = V.ceil,
    w = V.max,
    z = V.min,
    M = V.abs,
    da = V.cos,
    ia = V.sin,
    ra = V.PI,
    Ma = ra * 2 / 360,
    Da = navigator.userAgent,
    Sb = X.opera,
    Ha = /msie/i.test(Da) && !Sb,
    lb = B.documentMode === 8,
    mb = /AppleWebKit/.test(Da),
    $a = /Firefox/.test(Da),
    cb = /(Mobile|Android|Windows Phone)/.test(Da),
    Ea = "http://www.w3.org/2000/svg",
    ca = !!B.createElementNS && !!B.createElementNS(Ea, "svg").createSVGRect,
    Yb = $a && parseInt(Da.split("Firefox/")[1], 10) < 4,
    ja = !ca && !Ha && !!B.createElement("canvas").getContext,
    Wa,
    ab,
    Tb = {},
    Ab = 0,
    hb,
    F,
    ua,
    Ca,
    Bb,
    H,
    na = function() {},
    aa = [],
    db = 0,
    Ta = "div",
    Y = "none",
    Zb = /^[0-9]+$/,
    $b = "stroke-width",
    ib,
    La,
    ub,
    vb,
    wb,
    Ua,
    jb,
    kb,
    Nb,
    Ob,
    xb,
    Pb,
    Qb,
    C = {},
    P = X.Highcharts = X.Highcharts ? qa(16, !0) : {};
    ua = function(a, b, c) {
        if (!t(b) || isNaN(b)) return "Invalid date";
        var a = o(a, "%Y-%m-%d %H:%M:%S"),
        d = new Date(b - La),
        e,
        f = d[vb](),
        g = d[wb](),
        h = d[Ua](),
        i = d[jb](),
        k = d[kb](),
        j = F.lang,
        l = j.weekdays,
        d = v({
            a: l[g].substr(0, 3),
            A: l[g],
            d: Qa(h),
            e: h,
            b: j.shortMonths[i],
            B: j.months[i],
            m: Qa(i + 1),
            y: k.toString().substr(2, 2),
            Y: k,
            H: Qa(f),
            I: Qa(f % 12 || 12),
            l: f % 12 || 12,
            M: Qa(d[ub]()),
            p: f < 12 ? "AM": "PM",
            P: f < 12 ? "am": "pm",
            S: Qa(d.getSeconds()),
            L: Qa(s(b % 1E3), 3)
        },
        P.dateFormats);
        for (e in d) for (; a.indexOf("%" + e) !== -1;) a = a.replace("%" + e, typeof d[e] === "function" ? d[e](b) : d[e]);
        return c ? a.substr(0, 1).toUpperCase() + a.substr(1) : a
    };
    Lb.prototype = {
        wrapColor: function(a) {
            if (this.color >= a) this.color = 0
        },
        wrapSymbol: function(a) {
            if (this.symbol >= a) this.symbol = 0
        }
    };
    H = Kb("millisecond", 1, "second", 1E3, "minute", 6E4, "hour", 36E5, "day", 864E5, "week", 6048E5, "month", 26784E5, "year", 31556952E3);
    Bb = {
        init: function(a, b, c) {
            var b = b || "",
            d = a.shift,
            e = b.indexOf("C") > -1,
            f = e ? 7 : 3,
            g,
            b = b.split(" "),
            c = [].concat(c),
            h,
            i,
            k = function(a) {
                for (g = a.length; g--;) a[g] === "M" && a.splice(g + 1, 0, a[g + 1], a[g + 2], a[g + 1], a[g + 2])
            };
            e && (k(b), k(c));
            a.isArea && (h = b.splice(b.length - 6, 6), i = c.splice(c.length - 6, 6));
            if (d <= c.length / f && b.length === c.length) for (; d--;) c = [].concat(c).splice(0, f).concat(c);
            a.shift = 0;
            if (b.length) for (a = c.length; b.length < a;) d = [].concat(b).splice(b.length - f, f),
            e && (d[f - 6] = d[f - 2], d[f - 5] = d[f - 1]),
            b = b.concat(d);
            h && (b = b.concat(h), c = c.concat(i));
            return [b, c]
        },
        step: function(a, b, c, d) {
            var e = [],
            f = a.length;
            if (c === 1) e = d;
            else if (f === b.length && c < 1) for (; f--;) d = parseFloat(a[f]),
            e[f] = isNaN(d) ? a[f] : c * parseFloat(b[f] - d) + d;
            else e = b;
            return e
        }
    }; (function(a) {
        X.HighchartsAdapter = X.HighchartsAdapter || a && {
            init: function(b) {
                var c = a.fx,
                d = c.step,
                e, f = a.Tween,
                g = f && f.propHooks;
                e = a.cssHooks.opacity;
                a.extend(a.easing, {
                    easeOutQuad: function(a, b, c, d, e) {
                        return - d * (b /= e) * (b - 2) + c
                    }
                });
                a.each(["cur", "_default", "width", "height", "opacity"],
                function(a, b) {
                    var e = d,
                    j;
                    b === "cur" ? e = c.prototype: b === "_default" && f && (e = g[b], b = "set"); (j = e[b]) && (e[b] = function(c) {
                        var d, c = a ? c: this;
                        if (c.prop !== "align") return d = c.elem,
                        d.attr ? d.attr(c.prop, b === "cur" ? r: c.now) : j.apply(this, arguments)
                    })
                });
                O(e, "get",
                function(a, b, c) {
                    return b.attr ? b.opacity || 0 : a.call(this, b, c)
                });
                e = function(a) {
                    var c = a.elem,
                    d;
                    if (!a.started) d = b.init(c, c.d, c.toD),
                    a.start = d[0],
                    a.end = d[1],
                    a.started = !0;
                    c.attr("d", b.step(a.start, a.end, a.pos, c.toD))
                };
                f ? g.d = {
                    set: e
                }: d.d = e;
                this.each = Array.prototype.forEach ?
                function(a, b) {
                    return Array.prototype.forEach.call(a, b)
                }: function(a, b) {
                    for (var c = 0,
                    d = a.length; c < d; c++) if (b.call(a[c], a[c], c, a) === !1) return c
                };
                a.fn.highcharts = function() {
                    var a = "Chart",
                    b = arguments,
                    c, d;
                    if (this[0]) {
                        Oa(b[0]) && (a = b[0], b = Array.prototype.slice.call(b, 1));
                        c = b[0];
                        if (c !== r) c.chart = c.chart || {},
                        c.chart.renderTo = this[0],
                        new P[a](c, b[1]),
                        d = this;
                        c === r && (d = aa[W(this[0], "data-highcharts-chart")])
                    }
                    return d
                }
            },
            getScript: a.getScript,
            inArray: a.inArray,
            adapterRun: function(b, c) {
                return a(b)[c]()
            },
            grep: a.grep,
            map: function(a, c) {
                for (var d = [], e = 0, f = a.length; e < f; e++) d[e] = c.call(a[e], a[e], e, a);
                return d
            },
            offset: function(b) {
                return a(b).offset()
            },
            addEvent: function(b, c, d) {
                a(b).bind(c, d)
            },
            removeEvent: function(b, c, d) {
                var e = B.removeEventListener ? "removeEventListener": "detachEvent";
                B[e] && b && !b[e] && (b[e] = function() {});
                a(b).unbind(c, d)
            },
            fireEvent: function(b, c, d, e) {
                var f = a.Event(c),
                g = "detached" + c,
                h; ! Ha && d && (delete d.layerX, delete d.layerY, delete d.returnValue);
                v(f, d);
                b[c] && (b[g] = b[c], b[c] = null);
                a.each(["preventDefault", "stopPropagation"],
                function(a, b) {
                    var c = f[b];
                    f[b] = function() {
                        try {
                            c.call(f)
                        } catch(a) {
                            b === "preventDefault" && (h = !0)
                        }
                    }
                });
                a(b).trigger(f);
                b[g] && (b[c] = b[g], b[g] = null);
                e && !f.isDefaultPrevented() && !h && e(f)
            },
            washMouseEvent: function(a) {
                var c = a.originalEvent || a;
                if (c.pageX === r) c.pageX = a.pageX,
                c.pageY = a.pageY;
                return c
            },
            animate: function(b, c, d) {
                var e = a(b);
                if (!b.style) b.style = {};
                if (c.d) b.toD = c.d,
                c.d = 1;
                e.stop();
                c.opacity !== r && b.attr && (c.opacity += "px");
                e.animate(c, d)
            },
            stop: function(b) {
                a(b).stop()
            }
        }
    })(X.jQuery);
    var J = X.HighchartsAdapter,
    Q = J || {};
    J && J.init.call(J, Bb);
    var nb = Q.adapterRun,
    ac = Q.getScript,
    wa = Q.inArray,
    q = Q.each,
    Cb = Q.grep,
    bc = Q.offset,
    xa = Q.map,
    A = Q.addEvent,
    R = Q.removeEvent,
    N = Q.fireEvent,
    cc = Q.washMouseEvent,
    ob = Q.animate,
    eb = Q.stop,
    Q = {
        enabled: !0,
        x: 0,
        y: 15,
        style: {
            color: "#606060",
            cursor: "default",
            fontSize: "11px"
        }
    };
    F = {
        colors: "#7cb5ec,#434348,#90ed7d,#f7a35c,#8085e9,#f15c80,#e4d354,#8085e8,#8d4653,#91e8e1".split(","),
        symbols: ["circle", "diamond", "square", "triangle", "triangle-down"],
        lang: {
            loading: "Loading...",
            months: "January,February,March,April,May,June,July,August,September,October,November,December".split(","),
            shortMonths: "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".split(","),
            weekdays: "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday".split(","),
            decimalPoint: ".",
            numericSymbols: "k,M,G,T,P,E".split(","),
            resetZoom: "Reset zoom",
            resetZoomTitle: "Reset zoom level 1:1",
            thousandsSep: ","
        },
        global: {
            useUTC: !0,
            canvasToolsURL: "http://code.highcharts.com/stock/2.0.1/modules/canvas-tools.js",
            VMLRadialGradientURL: "http://code.highcharts.com/stock/2.0.1/gfx/vml-radial-gradient.png"
        },
        chart: {
            borderColor: "#4572A7",
            borderRadius: 0,
            defaultSeriesType: "line",
            ignoreHiddenSeries: !0,
            spacing: [10, 10, 15, 10],
            backgroundColor: "#0a0a0a",
            plotBorderColor: "#C0C0C0",
            resetZoomButton: {
                theme: {
                    zIndex: 20
                },
                position: {
                    align: "right",
                    x: -10,
                    y: 10
                }
            }
        },
        title: {
            text: "Chart title",
            align: "center",
            margin: 15,
            style: {
                color: "#333333",
                fontSize: "18px"
            }
        },
        subtitle: {
            text: "",
            align: "center",
            style: {
                color: "#555555"
            }
        },
        plotOptions: {
            line: {
                allowPointSelect: !1,
                showCheckbox: !1,
                animation: {
                    duration: 1E3
                },
                events: {},
                lineWidth: 2,
                marker: {
                    lineWidth: 0,
                    radius: 4,
                    lineColor: "#FFFFFF",
                    states: {
                        hover: {
                            enabled: !0
                        },
                        select: {
                            fillColor: "#FFFFFF",
                            lineColor: "#000000",
                            lineWidth: 2
                        }
                    }
                },
                point: {
                    events: {}
                },
                dataLabels: y(Q, {
                    align: "center",
                    enabled: !1,
                    formatter: function() {
                        return this.y === null ? "": Ia(this.y, -1)
                    },
                    verticalAlign: "bottom",
                    y: 0
                }),
                cropThreshold: 300,
                pointRange: 0,
                states: {
                    hover: {
                        marker: {},
                        halo: {
                            size: 10,
                            opacity: 0.25
                        }
                    },
                    select: {
                        marker: {}
                    }
                },
                stickyTracking: !0,
                turboThreshold: 1E3
            }
        },
        labels: {
            style: {
                position: "absolute",
                color: "#3E576F"
            }
        },
        legend: {
            enabled: !0,
            align: "center",
            layout: "horizontal",
            labelFormatter: function() {
                return this.name
            },
            borderColor: "#909090",
            borderRadius: 0,
            navigation: {
                activeColor: "#274b6d",
                inactiveColor: "#CCC"
            },
            shadow: !1,
            itemStyle: {
                color: "#333333",
                fontSize: "12px",
                fontWeight: "bold"
            },
            itemHoverStyle: {
                color: "#000"
            },
            itemHiddenStyle: {
                color: "#CCC"
            },
            itemCheckboxStyle: {
                position: "absolute",
                width: "13px",
                height: "13px"
            },
            symbolPadding: 5,
            verticalAlign: "bottom",
            x: 0,
            y: 0,
            title: {
                style: {
                    fontWeight: "bold"
                }
            }
        },
        loading: {
            labelStyle: {
                fontWeight: "bold",
                position: "relative",
                top: "1em"
            },
            style: {
                position: "absolute",
                backgroundColor: "white",
                opacity: 0.5,
                textAlign: "center"
            }
        },
        tooltip: {
            enabled: !0,
            animation: ca,
            backgroundColor: "rgba(249, 249, 249, .85)",
            borderWidth: 1,
            borderRadius: 3,
            dateTimeLabelFormats: {
                millisecond: "%A, %b %e, %H:%M:%S.%L",
                second: "%A, %b %e, %H:%M:%S",
                minute: "%A, %b %e, %H:%M",
                hour: "%A, %b %e, %H:%M",
                day: "%A, %b %e, %Y",
                week: "Week from %A, %b %e, %Y",
                month: "%B %Y",
                year: "%Y"
            },
            headerFormat: '<span style="font-size: 10px">{point.key}</span><br/>',
            pointFormat: '<span style="color:{series.color}">●</span> {series.name}: <b>{point.y}</b><br/>',
            shadow: !0,
            snap: cb ? 25 : 10,
            style: {
                color: "#333333",
                cursor: "default",
                fontSize: "12px",
                padding: "8px",
                whiteSpace: "nowrap"
            }
        },
        credits: {
            enabled: !0,
            text: "",
            href: "###",
            position: {
                align: "right",
                x: -10,
                verticalAlign: "bottom",
                y: -5
            },
            style: {
                cursor: "pointer",
                color: "#909090",
                fontSize: "9px"
            }
        }
    };
    var T = F.plotOptions,
    J = T.line;
    Mb();
    var dc = /rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]?(?:\.[0-9]+)?)\s*\)/,
    ec = /#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/,
    fc = /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/,
    Fa = function(a) {
        var b = [],
        c,
        d; (function(a) {
            a && a.stops ? d = xa(a.stops,
            function(a) {
                return Fa(a[1])
            }) : (c = dc.exec(a)) ? b = [I(c[1]), I(c[2]), I(c[3]), parseFloat(c[4], 10)] : (c = ec.exec(a)) ? b = [I(c[1], 16), I(c[2], 16), I(c[3], 16), 1] : (c = fc.exec(a)) && (b = [I(c[1]), I(c[2]), I(c[3]), 1])
        })(a);
        return {
            get: function(c) {
                var f;
                d ? (f = y(a), f.stops = [].concat(f.stops), q(d,
                function(a, b) {
                    f.stops[b] = [f.stops[b][0], a.get(c)]
                })) : f = b && !isNaN(b[0]) ? c === "rgb" ? "rgb(" + b[0] + "," + b[1] + "," + b[2] + ")": c === "a" ? b[3] : "rgba(" + b.join(",") + ")": a;
                return f
            },
            brighten: function(a) {
                if (d) q(d,
                function(b) {
                    b.brighten(a)
                });
                else if (la(a) && a !== 0) {
                    var c;
                    for (c = 0; c < 3; c++) b[c] += I(a * 255),
                    b[c] < 0 && (b[c] = 0),
                    b[c] > 255 && (b[c] = 255)
                }
                return this
            },
            rgba: b,
            setOpacity: function(a) {
                b[3] = a;
                return this
            }
        }
    };
    Z.prototype = {
        init: function(a, b) {
            this.element = b === "span" ? $(b) : B.createElementNS(Ea, b);
            this.renderer = a
        },
        opacity: 1,
        animate: function(a, b, c) {
            b = o(b, Ca, !0);
            eb(this);
            if (b) {
                b = y(b, {});
                if (c) b.complete = c;
                ob(this, a, b)
            } else this.attr(a),
            c && c()
        },
        colorGradient: function(a, b, c) {
            var d = this.renderer,
            e, f, g, h, i, k, j, l, m, n, p = [];
            a.linearGradient ? f = "linearGradient": a.radialGradient && (f = "radialGradient");
            if (f) {
                g = a[f];
                h = d.gradients;
                k = a.stops;
                m = c.radialReference;
                Pa(g) && (a[f] = g = {
                    x1: g[0],
                    y1: g[1],
                    x2: g[2],
                    y2: g[3],
                    gradientUnits: "userSpaceOnUse"
                });
                f === "radialGradient" && m && !t(g.gradientUnits) && (g = y(g, {
                    cx: m[0] - m[2] / 2 + g.cx * m[2],
                    cy: m[1] - m[2] / 2 + g.cy * m[2],
                    r: g.r * m[2],
                    gradientUnits: "userSpaceOnUse"
                }));
                for (n in g) n !== "id" && p.push(n, g[n]);
                for (n in k) p.push(k[n]);
                p = p.join(",");
                h[p] ? a = h[p].attr("id") : (g.id = a = "highcharts-" + Ab++, h[p] = i = d.createElement(f).attr(g).add(d.defs), i.stops = [], q(k,
                function(a) {
                    a[1].indexOf("rgba") === 0 ? (e = Fa(a[1]), j = e.get("rgb"), l = e.get("a")) : (j = a[1], l = 1);
                    a = d.createElement("stop").attr({
                        offset: a[0],
                        "stop-color": j,
                        "stop-opacity": l
                    }).add(i);
                    i.stops.push(a)
                }));
                c.setAttribute(b, "url(" + d.url + "#" + a + ")")
            }
        },
        attr: function(a, b) {
            var c, d, e = this.element,
            f, g = this,
            h;
            typeof a === "string" && b !== r && (c = a, a = {},
            a[c] = b);
            if (typeof a === "string") g = (this[a + "Getter"] || this._defaultGetter).call(this, a, e);
            else {
                for (c in a) {
                    d = a[c];
                    h = !1;
                    this.symbolName && /^(x|y|width|height|r|start|end|innerR|anchorX|anchorY)/.test(c) && (f || (this.symbolAttr(a), f = !0), h = !0);
                    if (this.rotation && (c === "x" || c === "y")) this.doTransform = !0;
                    h || (this[c + "Setter"] || this._defaultSetter).call(this, d, c, e);
                    this.shadows && /^(width|height|visibility|x|y|d|transform|cx|cy|r)$/.test(c) && this.updateShadows(c, d)
                }
                if (this.doTransform) this.updateTransform(),
                this.doTransform = !1
            }
            return g
        },
        updateShadows: function(a, b) {
            for (var c = this.shadows,
            d = c.length; d--;) c[d].setAttribute(a, a === "height" ? w(b - (c[d].cutHeight || 0), 0) : a === "d" ? this.d: b)
        },
        addClass: function(a) {
            var b = this.element,
            c = W(b, "class") || "";
            c.indexOf(a) === -1 && W(b, "class", c + " " + a);
            return this
        },
        symbolAttr: function(a) {
            var b = this;
            q("x,y,r,start,end,width,height,innerR,anchorX,anchorY".split(","),
            function(c) {
                b[c] = o(a[c], b[c])
            });
            b.attr({
                d: b.renderer.symbols[b.symbolName](b.x, b.y, b.width, b.height, b)
            })
        },
        clip: function(a) {
            return this.attr("clip-path", a ? "url(" + this.renderer.url + "#" + a.id + ")": Y)
        },
        crisp: function(a) {
            var b, c = {},
            d, e = a.strokeWidth || this.strokeWidth || this.attr && this.attr("stroke-width") || 0;
            d = s(e) % 2 / 2;
            a.x = S(a.x || this.x || 0) + d;
            a.y = S(a.y || this.y || 0) + d;
            a.width = S((a.width || this.width || 0) - 2 * d);
            a.height = S((a.height || this.height || 0) - 2 * d);
            a.strokeWidth = e;
            for (b in a) this[b] !== a[b] && (this[b] = c[b] = a[b]);
            return c
        },
        css: function(a) {
            var b = this.styles,
            c = {},
            d = this.element,
            e, f, g = "";
            e = !b;
            if (a && a.color) a.fill = a.color;
            if (b) for (f in a) a[f] !== b[f] && (c[f] = a[f], e = !0);
            if (e) {
                e = this.textWidth = a && a.width && d.nodeName.toLowerCase() === "text" && I(a.width);
                b && (a = v(b, c));
                this.styles = a;
                e && (ja || !ca && this.renderer.forExport) && delete a.width;
                if (Ha && !ca) E(this.element, a);
                else {
                    b = function(a, b) {
                        return "-" + b.toLowerCase()
                    };
                    for (f in a) g += f.replace(/([A-Z])/g, b) + ":" + a[f] + ";";
                    W(d, "style", g)
                }
                e && this.added && this.renderer.buildText(this)
            }
            return this
        },
        on: function(a, b) {
            var c = this,
            d = c.element;
            ab && a === "click" ? (d.ontouchstart = function(a) {
                c.touchEventFired = Date.now();
                a.preventDefault();
                b.call(d, a)
            },
            d.onclick = function(a) { (Da.indexOf("Android") === -1 || Date.now() - (c.touchEventFired || 0) > 1100) && b.call(d, a)
            }) : d["on" + a] = b;
            return this
        },
        setRadialReference: function(a) {
            this.element.radialReference = a;
            return this
        },
        translate: function(a, b) {
            return this.attr({
                translateX: a,
                translateY: b
            })
        },
        invert: function() {
            this.inverted = !0;
            this.updateTransform();
            return this
        },
        updateTransform: function() {
            var a = this.translateX || 0,
            b = this.translateY || 0,
            c = this.scaleX,
            d = this.scaleY,
            e = this.inverted,
            f = this.rotation,
            g = this.element;
            e && (a += this.attr("width"), b += this.attr("height"));
            a = ["translate(" + a + "," + b + ")"];
            e ? a.push("rotate(90) scale(-1,1)") : f && a.push("rotate(" + f + " " + (g.getAttribute("x") || 0) + " " + (g.getAttribute("y") || 0) + ")"); (t(c) || t(d)) && a.push("scale(" + o(c, 1) + " " + o(d, 1) + ")");
            a.length && g.setAttribute("transform", a.join(" "))
        },
        toFront: function() {
            var a = this.element;
            a.parentNode.appendChild(a);
            return this
        },
        align: function(a, b, c) {
            var d, e, f, g, h = {};
            e = this.renderer;
            f = e.alignedObjects;
            if (a) {
                if (this.alignOptions = a, this.alignByTranslate = b, !c || Oa(c)) this.alignTo = d = c || "renderer",
                pa(f, this),
                f.push(this),
                c = null
            } else a = this.alignOptions,
            b = this.alignByTranslate,
            d = this.alignTo;
            c = o(c, e[d], e);
            d = a.align;
            e = a.verticalAlign;
            f = (c.x || 0) + (a.x || 0);
            g = (c.y || 0) + (a.y || 0);
            if (d === "right" || d === "center") f += (c.width - (a.width || 0)) / {
                right: 1,
                center: 2
            } [d];
            h[b ? "translateX": "x"] = s(f);
            if (e === "bottom" || e === "middle") g += (c.height - (a.height || 0)) / ({
                bottom: 1,
                middle: 2
            } [e] || 1);
            h[b ? "translateY": "y"] = s(g);
            this[this.placed ? "animate": "attr"](h);
            this.placed = !0;
            this.alignAttr = h;
            return this
        },
        getBBox: function() {
            var a = this.bBox,
            b = this.renderer,
            c, d, e = this.rotation;
            c = this.element;
            var f = this.styles,
            g = e * Ma;
            d = this.textStr;
            var h;
            if (d === "" || Zb.test(d)) h = "num." + d.toString().length + (f ? "|" + f.fontSize + "|" + f.fontFamily: "");
            h && (a = b.cache[h]);
            if (!a) {
                if (c.namespaceURI === Ea || b.forExport) {
                    try {
                        a = c.getBBox ? v({},
                        c.getBBox()) : {
                            width: c.offsetWidth,
                            height: c.offsetHeight
                        }
                    } catch(i) {}
                    if (!a || a.width < 0) a = {
                        width: 0,
                        height: 0
                    }
                } else a = this.htmlGetBBox();
                if (b.isSVG) {
                    c = a.width;
                    d = a.height;
                    if (Ha && f && f.fontSize === "11px" && d.toPrecision(3) === "16.9") a.height = d = 14;
                    if (e) a.width = M(d * ia(g)) + M(c * da(g)),
                    a.height = M(d * da(g)) + M(c * ia(g))
                }
                this.bBox = a;
                h && (b.cache[h] = a)
            }
            return a
        },
        show: function(a) {
            return a && this.element.namespaceURI === Ea ? (this.element.removeAttribute("visibility"), this) : this.attr({
                visibility: a ? "inherit": "visible"
            })
        },
        hide: function() {
            return this.attr({
                visibility: "hidden"
            })
        },
        fadeOut: function(a) {
            var b = this;
            b.animate({
                opacity: 0
            },
            {
                duration: a || 150,
                complete: function() {
                    b.hide()
                }
            })
        },
        add: function(a) {
            var b = this.renderer,
            c = a || b,
            d = c.element || b.box,
            e = this.element,
            f = this.zIndex,
            g, h;
            if (a) this.parentGroup = a;
            this.parentInverted = a && a.inverted;
            this.textStr !== void 0 && b.buildText(this);
            if (f) c.handleZ = !0,
            f = I(f);
            if (c.handleZ) {
                a = d.childNodes;
                for (g = 0; g < a.length; g++) if (b = a[g], c = W(b, "zIndex"), b !== e && (I(c) > f || !t(f) && t(c))) {
                    d.insertBefore(e, b);
                    h = !0;
                    break
                }
            }
            h || d.appendChild(e);
            this.added = !0;
            if (this.onAdd) this.onAdd();
            return this
        },
        safeRemoveChild: function(a) {
            var b = a.parentNode;
            b && b.removeChild(a)
        },
        destroy: function() {
            var a = this,
            b = a.element || {},
            c = a.shadows,
            d = a.renderer.isSVG && b.nodeName === "SPAN" && a.parentGroup,
            e, f;
            b.onclick = b.onmouseout = b.onmouseover = b.onmousemove = b.point = null;
            eb(a);
            if (a.clipPath) a.clipPath = a.clipPath.destroy();
            if (a.stops) {
                for (f = 0; f < a.stops.length; f++) a.stops[f] = a.stops[f].destroy();
                a.stops = null
            }
            a.safeRemoveChild(b);
            for (c && q(c,
            function(b) {
                a.safeRemoveChild(b)
            }); d && d.div.childNodes.length === 0;) b = d.parentGroup,
            a.safeRemoveChild(d.div),
            delete d.div,
            d = b;
            a.alignTo && pa(a.renderer.alignedObjects, a);
            for (e in a) delete a[e];
            return null
        },
        shadow: function(a, b, c) {
            var d = [],
            e,
            f,
            g = this.element,
            h,
            i,
            k,
            j;
            if (a) {
                i = o(a.width, 3);
                k = (a.opacity || 0.15) / i;
                j = this.parentInverted ? "(-1,-1)": "(" + o(a.offsetX, 1) + ", " + o(a.offsetY, 1) + ")";
                for (e = 1; e <= i; e++) {
                    f = g.cloneNode(0);
                    h = i * 2 + 1 - 2 * e;
                    W(f, {
                        isShadow: "true",
                        stroke: a.color || "black",
                        "stroke-opacity": k * e,
                        "stroke-width": h,
                        transform: "translate" + j,
                        fill: Y
                    });
                    if (c) W(f, "height", w(W(f, "height") - h, 0)),
                    f.cutHeight = h;
                    b ? b.element.appendChild(f) : g.parentNode.insertBefore(f, g);
                    d.push(f)
                }
                this.shadows = d
            }
            return this
        },
        xGetter: function(a) {
            this.element.nodeName === "circle" && (a = {
                x: "cx",
                y: "cy"
            } [a] || a);
            return this._defaultGetter(a)
        },
        _defaultGetter: function(a) {
            a = o(this[a], this.element ? this.element.getAttribute(a) : null, 0);
            /^[0-9\.]+$/.test(a) && (a = parseFloat(a));
            return a
        },
        dSetter: function(a, b, c) {
            a && a.join && (a = a.join(" "));
            /(NaN| {2}|^$)/.test(a) && (a = "M 0 0");
            c.setAttribute(b, a);
            this[b] = a
        },
        dashstyleSetter: function(a) {
            var b;
            if (a = a && a.toLowerCase()) {
                a = a.replace("shortdashdotdot", "3,1,1,1,1,1,").replace("shortdashdot", "3,1,1,1").replace("shortdot", "1,1,").replace("shortdash", "3,1,").replace("longdash", "8,3,").replace(/dot/g, "1,3,").replace("dash", "4,3,").replace(/,$/, "").split(",");
                for (b = a.length; b--;) a[b] = I(a[b]) * this.element.getAttribute("stroke-width");
                a = a.join(",");
                this.element.setAttribute("stroke-dasharray", a)
            }
        },
        alignSetter: function(a) {
            this.element.setAttribute("text-anchor", {
                left: "start",
                center: "middle",
                right: "end"
            } [a])
        },
        opacitySetter: function(a, b, c) {
            this[b] = a;
            c.setAttribute(b, a)
        },
        "stroke-widthSetter": function(a, b, c) {
            a === 0 && (a = 1.0E-5);
            this.strokeWidth = a;
            c.setAttribute(b, a)
        },
        titleSetter: function(a) {
            var b = this.element.getElementsByTagName("title")[0];
            b || (b = B.createElementNS(Ea, "title"), this.element.appendChild(b));
            b.textContent = a
        },
        textSetter: function(a) {
            if (a !== this.textStr) delete this.bBox,
            this.textStr = a,
            this.added && this.renderer.buildText(this)
        },
        fillSetter: function(a, b, c) {
            typeof a === "string" ? c.setAttribute(b, a) : a && this.colorGradient(a, b, c)
        },
        zIndexSetter: function(a, b, c) {
            c.setAttribute(b, a);
            this[b] = a
        },
        _defaultSetter: function(a, b, c) {
            c.setAttribute(b, a)
        }
    };
    Z.prototype.yGetter = Z.prototype.xGetter;
    Z.prototype.translateXSetter = Z.prototype.translateYSetter = Z.prototype.rotationSetter = Z.prototype.verticalAlignSetter = Z.prototype.scaleXSetter = Z.prototype.scaleYSetter = function(a, b) {
        this[b] = a;
        this.doTransform = !0
    };
    Z.prototype.strokeSetter = Z.prototype.fillSetter;
    var ka = function() {
        this.init.apply(this, arguments)
    };
    ka.prototype = {
        Element: Z,
        init: function(a, b, c, d, e) {
            var f = location,
            g, d = this.createElement("svg").attr({
                version: "1.1"
            }).css(this.getStyle(d));
            g = d.element;
            a.appendChild(g);
            a.innerHTML.indexOf("xmlns") === -1 && W(g, "xmlns", Ea);
            this.isSVG = !0;
            this.box = g;
            this.boxWrapper = d;
            this.alignedObjects = [];
            this.url = ($a || mb) && B.getElementsByTagName("base").length ? f.href.replace(/#.*?$/, "").replace(/([\('\)])/g, "\\$1").replace(/ /g, "%20") : "";
            this.createElement("desc").add().element.appendChild(B.createTextNode("Created with Highstock 2.0.1"));
            this.defs = this.createElement("defs").add();
            this.forExport = e;
            this.gradients = {};
            this.cache = {};
            this.setSize(b, c, !1);
            var h;
            if ($a && a.getBoundingClientRect) this.subPixelFix = b = function() {
                E(a, {
                    left: 0,
                    top: 0
                });
                h = a.getBoundingClientRect();
                E(a, {
                    left: Va(h.left) - h.left + "px",
                    top: Va(h.top) - h.top + "px"
                })
            },
            b(),
            A(X, "resize", b)
        },
        getStyle: function(a) {
            return this.style = v({
                fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Arial, Helvetica, sans-serif',
                fontSize: "12px"
            },
            a)
        },
        isHidden: function() {
            return ! this.boxWrapper.getBBox().width
        },
        destroy: function() {
            var a = this.defs;
            this.box = null;
            this.boxWrapper = this.boxWrapper.destroy();
            Ka(this.gradients || {});
            this.gradients = null;
            if (a) this.defs = a.destroy();
            this.subPixelFix && R(X, "resize", this.subPixelFix);
            return this.alignedObjects = null
        },
        createElement: function(a) {
            var b = new this.Element;
            b.init(this, a);
            return b
        },
        draw: function() {},
        buildText: function(a) {
            for (var b = a.element,
            c = this,
            d = c.forExport,
            e = o(a.textStr, "").toString(), f = e.indexOf("<") !== -1, g = b.childNodes, h, i, k = W(b, "x"), j = a.styles, l = a.textWidth, m = j && j.lineHeight, n = g.length, p = function(a) {
                return m ? I(m) : c.fontMetrics(/(px|em)$/.test(a && a.style.fontSize) ? a.style.fontSize: j && j.fontSize || c.style.fontSize || 12).h
            }; n--;) b.removeChild(g[n]); ! f && e.indexOf(" ") === -1 ? b.appendChild(B.createTextNode(e)) : (h = /<.*style="([^"]+)".*>/, i = /<.*href="(http[^"]+)".*>/, l && !a.added && this.box.appendChild(b), e = f ? e.replace(/<(b|strong)>/g, '<span style="font-weight:bold">').replace(/<(i|em)>/g, '<span style="font-style:italic">').replace(/<a/g, "<span").replace(/<\/(b|strong|i|em|a)>/g, "</span>").split(/<br.*?>/g) : [e], e[e.length - 1] === "" && e.pop(), q(e,
            function(e, f) {
                var g, m = 0,
                e = e.replace(/<span/g, "|||<span").replace(/<\/span>/g, "</span>|||");
                g = e.split("|||");
                q(g,
                function(e) {
                    if (e !== "" || g.length === 1) {
                        var n = {},
                        u = B.createElementNS(Ea, "tspan"),
                        o;
                        h.test(e) && (o = e.match(h)[1].replace(/(;| |^)color([ :])/, "$1fill$2"), W(u, "style", o));
                        i.test(e) && !d && (W(u, "onclick", 'location.href="' + e.match(i)[1] + '"'), E(u, {
                            cursor: "pointer"
                        }));
                        e = (e.replace(/<(.|\n)*?>/g, "") || " ").replace(/&lt;/g, "<").replace(/&gt;/g, ">");
                        if (e !== " ") {
                            u.appendChild(B.createTextNode(e));
                            if (m) n.dx = 0;
                            else if (f && k !== null) n.x = k;
                            W(u, n); ! m && f && (!ca && d && E(u, {
                                display: "block"
                            }), W(u, "dy", p(u), mb && u.offsetHeight));
                            b.appendChild(u);
                            m++;
                            if (l) for (var e = e.replace(/([^\^])-/g, "$1- ").split(" "), n = e.length > 1 && j.whiteSpace !== "nowrap", q, r, D = a._clipHeight, t = [], s = p(), w = 1; n && (e.length || t.length);) delete a.bBox,
                            q = a.getBBox(),
                            r = q.width,
                            !ca && c.forExport && (r = c.measureSpanWidth(u.firstChild.data, a.styles)),
                            q = r > l,
                            !q || e.length === 1 ? (e = t, t = [], e.length && (w++, D && w * s > D ? (e = ["..."], a.attr("title", a.textStr)) : (u = B.createElementNS(Ea, "tspan"), W(u, {
                                dy: s,
                                x: k
                            }), o && W(u, "style", o), b.appendChild(u), r > l && (l = r)))) : (u.removeChild(u.firstChild), t.unshift(e.pop())),
                            e.length && u.appendChild(B.createTextNode(e.join(" ").replace(/- /g, "-")))
                        }
                    }
                })
            }))
        },
        button: function(a, b, c, d, e, f, g, h, i) {
            var k = this.label(a, b, c, i, null, null, null, null, "button"),
            j = 0,
            l,
            m,
            n,
            p,
            u,
            o,
            a = {
                x1: 0,
                y1: 0,
                x2: 0,
                y2: 1
            },
            e = y({
                "stroke-width": 1,
                stroke: "#444",
                fill: {
                    linearGradient: a,
                    stops: [[0, "#FEFEFE"], [1, "#F6F6F6"]]
                },
                r: 2,
                padding: 5,
                style: {
                    color: "black"
                }
            },
            e);
            n = e.style;
            delete e.style;
            f = y(e, {
                stroke: "#68A",
                fill: {
                    linearGradient: a,
                    stops: [[0, "#FFF"], [1, "#ACF"]]
                }
            },
            f);
            p = f.style;
            delete f.style;
            g = y(e, {
                stroke: "#68A",
                fill: {
                    linearGradient: a,
                    stops: [[0, "#9BD"], [1, "#CDF"]]
                }
            },
            g);
            u = g.style;
            delete g.style;
            h = y(e, {
                style: {
                    color: "#666"
                }
            },
            h);
            o = h.style;
            delete h.style;
            A(k.element, Ha ? "mouseover": "mouseenter",
            function() {
                j !== 3 && k.attr(f).css(p)
            });
            A(k.element, Ha ? "mouseout": "mouseleave",
            function() {
                j !== 3 && (l = [e, f, g][j], m = [n, p, u][j], k.attr(l).css(m))
            });
            k.setState = function(a) { (k.state = j = a) ? a === 2 ? k.attr(g).css(u) : a === 3 && k.attr(h).css(o) : k.attr(e).css(n)
            };
            return k.on("click",
            function() {
                j !== 3 && d.call(k)
            }).attr(e).css(v({
                cursor: "default"
            },
            n))
        },
        crispLine: function(a, b) {
            a[1] === a[4] && (a[1] = a[4] = s(a[1]) - b % 2 / 2);
            a[2] === a[5] && (a[2] = a[5] = s(a[2]) + b % 2 / 2);
            return a
        },
        path: function(a) {
            var b = {
                fill: Y
            };
            Pa(a) ? b.d = a: fa(a) && v(b, a);
            return this.createElement("path").attr(b)
        },
        circle: function(a, b, c) {
            a = fa(a) ? a: {
                x: a,
                y: b,
                r: c
            };
            b = this.createElement("circle");
            b.xSetter = function(a) {
                this.element.setAttribute("cx", a)
            };
            b.ySetter = function(a) {
                this.element.setAttribute("cy", a)
            };
            return b.attr(a)
        },
        arc: function(a, b, c, d, e, f) {
            if (fa(a)) b = a.y,
            c = a.r,
            d = a.innerR,
            e = a.start,
            f = a.end,
            a = a.x;
            a = this.symbol("arc", a || 0, b || 0, c || 0, c || 0, {
                innerR: d || 0,
                start: e || 0,
                end: f || 0
            });
            a.r = c;
            return a
        },
        rect: function(a, b, c, d, e, f) {
            var e = fa(a) ? a.r: e,
            g = this.createElement("rect"),
            a = fa(a) ? a: a === r ? {}: {
                x: a,
                y: b,
                width: w(c, 0),
                height: w(d, 0)
            };
            if (f !== r) a.strokeWidth = f,
            a = g.crisp(a);
            if (e) a.r = e;
            g.rSetter = function(a) {
                W(this.element, {
                    rx: a,
                    ry: a
                })
            };
            return g.attr(a)
        },
        setSize: function(a, b, c) {
            var d = this.alignedObjects,
            e = d.length;
            this.width = a;
            this.height = b;
            for (this.boxWrapper[o(c, !0) ? "animate": "attr"]({
                width: a,
                height: b
            }); e--;) d[e].align()
        },
        g: function(a) {
            var b = this.createElement("g");
            return t(a) ? b.attr({
                "class": "highcharts-" + a
            }) : b
        },
        image: function(a, b, c, d, e) {
            var f = {
                preserveAspectRatio: Y
            };
            arguments.length > 1 && v(f, {
                x: b,
                y: c,
                width: d,
                height: e
            });
            f = this.createElement("image").attr(f);
            f.element.setAttributeNS ? f.element.setAttributeNS("http://www.w3.org/1999/xlink", "href", a) : f.element.setAttribute("hc-svg-href", a);
            return f
        },
        symbol: function(a, b, c, d, e, f) {
            var g, h = this.symbols[a],
            h = h && h(s(b), s(c), d, e, f),
            i = /^url\((.*?)\)$/,
            k,
            j;
            if (h) g = this.path(h),
            v(g, {
                symbolName: a,
                x: b,
                y: c,
                width: d,
                height: e
            }),
            f && v(g, f);
            else if (i.test(a)) j = function(a, b) {
                a.element && (a.attr({
                    width: b[0],
                    height: b[1]
                }), a.alignByTranslate || a.translate(s((d - b[0]) / 2), s((e - b[1]) / 2)))
            },
            k = a.match(i)[1],
            a = Tb[k],
            g = this.image(k).attr({
                x: b,
                y: c
            }),
            g.isImg = !0,
            a ? j(g, a) : (g.attr({
                width: 0,
                height: 0
            }), $("img", {
                onload: function() {
                    j(g, Tb[k] = [this.width, this.height])
                },
                src: k
            }));
            return g
        },
        symbols: {
            circle: function(a, b, c, d) {
                var e = 0.166 * c;
                return ["M", a + c / 2, b, "C", a + c + e, b, a + c + e, b + d, a + c / 2, b + d, "C", a - e, b + d, a - e, b, a + c / 2, b, "Z"]
            },
            square: function(a, b, c, d) {
                return ["M", a, b, "L", a + c, b, a + c, b + d, a, b + d, "Z"]
            },
            triangle: function(a, b, c, d) {
                return ["M", a + c / 2, b, "L", a + c, b + d, a, b + d, "Z"]
            },
            "triangle-down": function(a, b, c, d) {
                return ["M", a, b, "L", a + c, b, a + c / 2, b + d, "Z"]
            },
            diamond: function(a, b, c, d) {
                return ["M", a + c / 2, b, "L", a + c, b + d / 2, a + c / 2, b + d, a, b + d / 2, "Z"]
            },
            arc: function(a, b, c, d, e) {
                var f = e.start,
                c = e.r || c || d,
                g = e.end - 0.001,
                d = e.innerR,
                h = e.open,
                i = da(f),
                k = ia(f),
                j = da(g),
                g = ia(g),
                e = e.end - f < ra ? 0 : 1;
                return ["M", a + c * i, b + c * k, "A", c, c, 0, e, 1, a + c * j, b + c * g, h ? "M": "L", a + d * j, b + d * g, "A", d, d, 0, e, 0, a + d * i, b + d * k, h ? "": "Z"]
            },
            callout: function(a, b, c, d, e) {
                var f = z(e && e.r || 0, c, d),
                g = f + 6,
                h = e && e.anchorX,
                i = e && e.anchorY,
                e = s(e.strokeWidth || 0) % 2 / 2;
                a += e;
                b += e;
                e = ["M", a + f, b, "L", a + c - f, b, "C", a + c, b, a + c, b, a + c, b + f, "L", a + c, b + d - f, "C", a + c, b + d, a + c, b + d, a + c - f, b + d, "L", a + f, b + d, "C", a, b + d, a, b + d, a, b + d - f, "L", a, b + f, "C", a, b, a, b, a + f, b];
                h && h > c && i > b + g && i < b + d - g ? e.splice(13, 3, "L", a + c, i - 6, a + c + 6, i, a + c, i + 6, a + c, b + d - f) : h && h < 0 && i > b + g && i < b + d - g ? e.splice(33, 3, "L", a, i + 6, a - 6, i, a, i - 6, a, b + f) : i && i > d && h > a + g && h < a + c - g ? e.splice(23, 3, "L", h + 6, b + d, h, b + d + 6, h - 6, b + d, a + f, b + d) : i && i < 0 && h > a + g && h < a + c - g && e.splice(3, 3, "L", h - 6, b, h, b - 6, h + 6, b, c - f, b);
                return e
            }
        },
        clipRect: function(a, b, c, d) {
            var e = "highcharts-" + Ab++,
            f = this.createElement("clipPath").attr({
                id: e
            }).add(this.defs),
            a = this.rect(a, b, c, d, 0).add(f);
            a.id = e;
            a.clipPath = f;
            return a
        },
        text: function(a, b, c, d) {
            var e = ja || !ca && this.forExport,
            f = {};
            if (d && !this.forExport) return this.html(a, b, c);
            f.x = Math.round(b || 0);
            if (c) f.y = Math.round(c);
            if (a || a === 0) f.text = a;
            a = this.createElement("text").attr(f);
            e && a.css({
                position: "absolute"
            });
            if (!d) a.xSetter = function(a, b, c) {
                var d = c.childNodes,
                e, f;
                for (f = 1; f < d.length; f++) e = d[f],
                e.getAttribute("x") === c.getAttribute("x") && e.setAttribute("x", a);
                c.setAttribute(b, a)
            };
            return a
        },
        fontMetrics: function(a) {
            var a = a || this.style.fontSize,
            a = /px/.test(a) ? I(a) : /em/.test(a) ? parseFloat(a) * 12 : 12,
            a = a < 24 ? a + 4 : s(a * 1.2),
            b = s(a * 0.8);
            return {
                h: a,
                b: b
            }
        },
        label: function(a, b, c, d, e, f, g, h, i) {
            function k() {
                var a, b;
                a = p.element.style;
                o = (bb === void 0 || Eb === void 0 || n.styles.textAlign) && p.textStr && p.getBBox();
                n.width = (bb || o.width || 0) + 2 * x + w;
                n.height = (Eb || o.height || 0) + 2 * x;
                U = x + m.fontMetrics(a && a.fontSize).b;
                if (z) {
                    if (!u) a = s( - G * x),
                    b = h ? -U: 0,
                    n.box = u = d ? m.symbol(d, a, b, n.width, n.height, D) : m.rect(a, b, n.width, n.height, 0, D[$b]),
                    u.attr("fill", Y).add(n);
                    u.isImg || u.attr(v({
                        width: s(n.width),
                        height: s(n.height)
                    },
                    D));
                    D = null
                }
            }
            function j() {
                var a = n.styles,
                a = a && a.textAlign,
                b = w + x * (1 - G),
                c;
                c = h ? 0 : U;
                if (t(bb) && o && (a === "center" || a === "right")) b += {
                    center: 0.5,
                    right: 1
                } [a] * (bb - o.width);
                if (b !== p.x || c !== p.y) p.attr("x", b),
                c !== r && p.attr("y", c);
                p.x = b;
                p.y = c
            }
            function l(a, b) {
                u ? u.attr(a, b) : D[a] = b
            }
            var m = this,
            n = m.g(i),
            p = m.text("", 0, 0, g).attr({
                zIndex: 1
            }),
            u,
            o,
            G = 0,
            x = 3,
            w = 0,
            bb,
            Eb,
            Fb,
            Gb,
            Ub = 0,
            D = {},
            U,
            z;
            n.onAdd = function() {
                p.add(n);
                n.attr({
                    text: a || "",
                    x: b,
                    y: c
                });
                u && t(e) && n.attr({
                    anchorX: e,
                    anchorY: f
                })
            };
            n.widthSetter = function(a) {
                bb = a
            };
            n.heightSetter = function(a) {
                Eb = a
            };
            n.paddingSetter = function(a) {
                t(a) && a !== x && (x = a, j())
            };
            n.paddingLeftSetter = function(a) {
                t(a) && a !== w && (w = a, j())
            };
            n.alignSetter = function(a) {
                G = {
                    left: 0,
                    center: 0.5,
                    right: 1
                } [a]
            };
            n.textSetter = function(a) {
                a !== r && p.textSetter(a);
                k();
                j()
            };
            n["stroke-widthSetter"] = function(a, b) {
                a && (z = !0);
                Ub = a % 2 / 2;
                l(b, a)
            };
            n.strokeSetter = n.fillSetter = n.rSetter = function(a, b) {
                b === "fill" && a && (z = !0);
                l(b, a)
            };
            n.anchorXSetter = function(a, b) {
                e = a;
                l(b, a + Ub - Fb)
            };
            n.anchorYSetter = function(a, b) {
                f = a;
                l(b, a - Gb)
            };
            n.xSetter = function(a) {
                n.x = a;
                G && (a -= G * ((bb || o.width) + x));
                Fb = s(a);
                n.attr("translateX", Fb)
            };
            n.ySetter = function(a) {
                Gb = n.y = s(a);
                n.attr("translateY", Gb)
            };
            var A = n.css;
            return v(n, {
                css: function(a) {
                    if (a) {
                        var b = {},
                        a = y(a);
                        q("fontSize,fontWeight,fontFamily,color,lineHeight,width,textDecoration,textShadow".split(","),
                        function(c) {
                            a[c] !== r && (b[c] = a[c], delete a[c])
                        });
                        p.css(b)
                    }
                    return A.call(n, a)
                },
                getBBox: function() {
                    return {
                        width: o.width + 2 * x,
                        height: o.height + 2 * x,
                        x: o.x - x,
                        y: o.y - x
                    }
                },
                shadow: function(a) {
                    u && u.shadow(a);
                    return n
                },
                destroy: function() {
                    R(n.element, "mouseenter");
                    R(n.element, "mouseleave");
                    p && (p = p.destroy());
                    u && (u = u.destroy());
                    Z.prototype.destroy.call(n);
                    n = m = k = j = l = null
                }
            })
        }
    };
    Wa = ka;
    v(Z.prototype, {
        htmlCss: function(a) {
            var b = this.element;
            if (b = a && b.tagName === "SPAN" && a.width) delete a.width,
            this.textWidth = b,
            this.updateTransform();
            this.styles = v(this.styles, a);
            E(this.element, a);
            return this
        },
        htmlGetBBox: function() {
            var a = this.element,
            b = this.bBox;
            if (!b) {
                if (a.nodeName === "text") a.style.position = "absolute";
                b = this.bBox = {
                    x: a.offsetLeft,
                    y: a.offsetTop,
                    width: a.offsetWidth,
                    height: a.offsetHeight
                }
            }
            return b
        },
        htmlUpdateTransform: function() {
            if (this.added) {
                var a = this.renderer,
                b = this.element,
                c = this.translateX || 0,
                d = this.translateY || 0,
                e = this.x || 0,
                f = this.y || 0,
                g = this.textAlign || "left",
                h = {
                    left: 0,
                    center: 0.5,
                    right: 1
                } [g],
                i = this.shadows;
                E(b, {
                    marginLeft: c,
                    marginTop: d
                });
                i && q(i,
                function(a) {
                    E(a, {
                        marginLeft: c + 1,
                        marginTop: d + 1
                    })
                });
                this.inverted && q(b.childNodes,
                function(c) {
                    a.invertChild(c, b)
                });
                if (b.tagName === "SPAN") {
                    var k = this.rotation,
                    j, l = I(this.textWidth),
                    m = [k, g, b.innerHTML, this.textWidth].join(",");
                    if (m !== this.cTT) {
                        j = a.fontMetrics(b.style.fontSize).b;
                        t(k) && this.setSpanRotation(k, h, j);
                        i = o(this.elemWidth, b.offsetWidth);
                        if (i > l && /[ \-]/.test(b.textContent || b.innerText)) E(b, {
                            width: l + "px",
                            display: "block",
                            whiteSpace: "normal"
                        }),
                        i = l;
                        this.getSpanCorrection(i, j, h, k, g)
                    }
                    E(b, {
                        left: e + (this.xCorr || 0) + "px",
                        top: f + (this.yCorr || 0) + "px"
                    });
                    if (mb) j = b.offsetHeight;
                    this.cTT = m
                }
            } else this.alignOnAdd = !0
        },
        setSpanRotation: function(a, b, c) {
            var d = {},
            e = Ha ? "-ms-transform": mb ? "-webkit-transform": $a ? "MozTransform": Sb ? "-o-transform": "";
            d[e] = d.transform = "rotate(" + a + "deg)";
            d[e + ($a ? "Origin": "-origin")] = d.transformOrigin = b * 100 + "% " + c + "px";
            E(this.element, d)
        },
        getSpanCorrection: function(a, b, c) {
            this.xCorr = -a * c;
            this.yCorr = -b
        }
    });
    v(ka.prototype, {
        html: function(a, b, c) {
            var d = this.createElement("span"),
            e = d.element,
            f = d.renderer;
            d.textSetter = function(a) {
                a !== e.innerHTML && delete this.bBox;
                e.innerHTML = this.textStr = a
            };
            d.xSetter = d.ySetter = d.alignSetter = d.rotationSetter = function(a, b) {
                b === "align" && (b = "textAlign");
                d[b] = a;
                d.htmlUpdateTransform()
            };
            d.attr({
                text: a,
                x: s(b),
                y: s(c)
            }).css({
                position: "absolute",
                whiteSpace: "nowrap",
                fontFamily: this.style.fontFamily,
                fontSize: this.style.fontSize
            });
            d.css = d.htmlCss;
            if (f.isSVG) d.add = function(a) {
                var b, c = f.box.parentNode,
                k = [];
                if (this.parentGroup = a) {
                    if (b = a.div, !b) {
                        for (; a;) k.push(a),
                        a = a.parentGroup;
                        q(k.reverse(),
                        function(a) {
                            var d;
                            b = a.div = a.div || $(Ta, {
                                className: W(a.element, "class")
                            },
                            {
                                position: "absolute",
                                left: (a.translateX || 0) + "px",
                                top: (a.translateY || 0) + "px"
                            },
                            b || c);
                            d = b.style;
                            v(a, {
                                translateXSetter: function(b, c) {
                                    d.left = b + "px";
                                    a[c] = b;
                                    a.doTransform = !0
                                },
                                translateYSetter: function(b, c) {
                                    d.top = b + "px";
                                    a[c] = b;
                                    a.doTransform = !0
                                },
                                visibilitySetter: function(a, b) {
                                    d[b] = a
                                }
                            })
                        })
                    }
                } else b = c;
                b.appendChild(e);
                d.added = !0;
                d.alignOnAdd && d.htmlUpdateTransform();
                return d
            };
            return d
        }
    });
    var fb, ea;
    if (!ca && !ja) P.VMLElement = ea = {
        init: function(a, b) {
            var c = ["<", b, ' filled="f" stroked="f"'],
            d = ["position: ", "absolute", ";"],
            e = b === Ta; (b === "shape" || e) && d.push("left:0;top:0;width:1px;height:1px;");
            d.push("visibility: ", e ? "hidden": "visible");
            c.push(' style="', d.join(""), '"/>');
            if (b) c = e || b === "span" || b === "img" ? c.join("") : a.prepVML(c),
            this.element = $(c);
            this.renderer = a
        },
        add: function(a) {
            var b = this.renderer,
            c = this.element,
            d = b.box,
            d = a ? a.element || a: d;
            a && a.inverted && b.invertChild(c, d);
            d.appendChild(c);
            this.added = !0;
            this.alignOnAdd && !this.deferUpdateTransform && this.updateTransform();
            if (this.onAdd) this.onAdd();
            return this
        },
        updateTransform: Z.prototype.htmlUpdateTransform,
        setSpanRotation: function() {
            var a = this.rotation,
            b = da(a * Ma),
            c = ia(a * Ma);
            E(this.element, {
                filter: a ? ["progid:DXImageTransform.Microsoft.Matrix(M11=", b, ", M12=", -c, ", M21=", c, ", M22=", b, ", sizingMethod='auto expand')"].join("") : Y
            })
        },
        getSpanCorrection: function(a, b, c, d, e) {
            var f = d ? da(d * Ma) : 1,
            g = d ? ia(d * Ma) : 0,
            h = o(this.elemHeight, this.element.offsetHeight),
            i;
            this.xCorr = f < 0 && -a;
            this.yCorr = g < 0 && -h;
            i = f * g < 0;
            this.xCorr += g * b * (i ? 1 - c: c);
            this.yCorr -= f * b * (d ? i ? c: 1 - c: 1);
            e && e !== "left" && (this.xCorr -= a * c * (f < 0 ? -1 : 1), d && (this.yCorr -= h * c * (g < 0 ? -1 : 1)), E(this.element, {
                textAlign: e
            }))
        },
        pathToVML: function(a) {
            for (var b = a.length,
            c = []; b--;) if (la(a[b])) c[b] = s(a[b] * 10) - 5;
            else if (a[b] === "Z") c[b] = "x";
            else if (c[b] = a[b], a.isArc && (a[b] === "wa" || a[b] === "at")) c[b + 5] === c[b + 7] && (c[b + 7] += a[b + 7] > a[b + 5] ? 1 : -1),
            c[b + 6] === c[b + 8] && (c[b + 8] += a[b + 8] > a[b + 6] ? 1 : -1);
            return c.join(" ") || "x"
        },
        clip: function(a) {
            var b = this,
            c;
            a ? (c = a.members, pa(c, b), c.push(b), b.destroyClip = function() {
                pa(c, b)
            },
            a = a.getCSS(b)) : (b.destroyClip && b.destroyClip(), a = {
                clip: lb ? "inherit": "rect(auto)"
            });
            return b.css(a)
        },
        css: Z.prototype.htmlCss,
        safeRemoveChild: function(a) {
            a.parentNode && Sa(a)
        },
        destroy: function() {
            this.destroyClip && this.destroyClip();
            return Z.prototype.destroy.apply(this)
        },
        on: function(a, b) {
            this.element["on" + a] = function() {
                var a = X.event;
                a.target = a.srcElement;
                b(a)
            };
            return this
        },
        cutOffPath: function(a, b) {
            var c, a = a.split(/[ ,]/);
            c = a.length;
            if (c === 9 || c === 11) a[c - 4] = a[c - 2] = I(a[c - 2]) - 10 * b;
            return a.join(" ")
        },
        shadow: function(a, b, c) {
            var d = [],
            e,
            f = this.element,
            g = this.renderer,
            h,
            i = f.style,
            k,
            j = f.path,
            l,
            m,
            n,
            p;
            j && typeof j.value !== "string" && (j = "x");
            m = j;
            if (a) {
                n = o(a.width, 3);
                p = (a.opacity || 0.15) / n;
                for (e = 1; e <= 3; e++) {
                    l = n * 2 + 1 - 2 * e;
                    c && (m = this.cutOffPath(j.value, l + 0.5));
                    k = ['<shape isShadow="true" strokeweight="', l, '" filled="false" path="', m, '" coordsize="10 10" style="', f.style.cssText, '" />'];
                    h = $(g.prepVML(k), null, {
                        left: I(i.left) + o(a.offsetX, 1),
                        top: I(i.top) + o(a.offsetY, 1)
                    });
                    if (c) h.cutOff = l + 1;
                    k = ['<stroke color="', a.color || "black", '" opacity="', p * e, '"/>'];
                    $(g.prepVML(k), null, null, h);
                    b ? b.element.appendChild(h) : f.parentNode.insertBefore(h, f);
                    d.push(h)
                }
                this.shadows = d
            }
            return this
        },
        updateShadows: na,
        setAttr: function(a, b) {
            lb ? this.element[a] = b: this.element.setAttribute(a, b)
        },
        classSetter: function(a) {
            this.element.className = a
        },
        dashstyleSetter: function(a, b, c) { (c.getElementsByTagName("stroke")[0] || $(this.renderer.prepVML(["<stroke/>"]), null, null, c))[b] = a || "solid";
            this[b] = a
        },
        dSetter: function(a, b, c) {
            var d = this.shadows,
            a = a || [];
            this.d = a.join(" ");
            c.path = a = this.pathToVML(a);
            if (d) for (c = d.length; c--;) d[c].path = d[c].cutOff ? this.cutOffPath(a, d[c].cutOff) : a;
            this.setAttr(b, a)
        },
        fillSetter: function(a, b, c) {
            var d = c.nodeName;
            if (d === "SPAN") c.style.color = a;
            else if (d !== "IMG") c.filled = a !== Y,
            this.setAttr("fillcolor", this.renderer.color(a, c, b, this))
        },
        opacitySetter: na,
        rotationSetter: function(a, b, c) {
            c = c.style;
            this[b] = c[b] = a;
            c.left = -s(ia(a * Ma) + 1) + "px";
            c.top = s(da(a * Ma)) + "px"
        },
        strokeSetter: function(a, b, c) {
            this.setAttr("strokecolor", this.renderer.color(a, c, b))
        },
        "stroke-widthSetter": function(a, b, c) {
            c.stroked = !!a;
            this[b] = a;
            la(a) && (a += "px");
            this.setAttr("strokeweight", a)
        },
        titleSetter: function(a, b) {
            this.setAttr(b, a)
        },
        visibilitySetter: function(a, b, c) {
            a === "inherit" && (a = "visible");
            this.shadows && q(this.shadows,
            function(c) {
                c.style[b] = a
            });
            c.nodeName === "DIV" && (a = a === "hidden" ? "-999em": 0, lb || (c.style[b] = a ? "visible": "hidden"), b = "top");
            c.style[b] = a
        },
        xSetter: function(a, b, c) {
            this[b] = a;
            b === "x" ? b = "left": b === "y" && (b = "top");
            this.updateClipping ? (this[b] = a, this.updateClipping()) : c.style[b] = a
        },
        zIndexSetter: function(a, b, c) {
            c.style[b] = a
        }
    },
    ea = ga(Z, ea),
    ea.prototype.ySetter = ea.prototype.widthSetter = ea.prototype.heightSetter = ea.prototype.xSetter,
    ea = {
        Element: ea,
        isIE8: Da.indexOf("MSIE 8.0") > -1,
        init: function(a, b, c, d) {
            var e;
            this.alignedObjects = [];
            d = this.createElement(Ta).css(v(this.getStyle(d), {
                position: "relative"
            }));
            e = d.element;
            a.appendChild(d.element);
            this.isVML = !0;
            this.box = e;
            this.boxWrapper = d;
            this.cache = {};
            this.setSize(b, c, !1);
            if (!B.namespaces.hcv) {
                B.namespaces.add("hcv", "urn:schemas-microsoft-com:vml");
                try {
                    B.createStyleSheet().cssText = "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } "
                } catch(f) {
                    B.styleSheets[0].cssText += "hcv\\:fill, hcv\\:path, hcv\\:shape, hcv\\:stroke{ behavior:url(#default#VML); display: inline-block; } "
                }
            }
        },
        isHidden: function() {
            return ! this.box.offsetWidth
        },
        clipRect: function(a, b, c, d) {
            var e = this.createElement(),
            f = fa(a);
            return v(e, {
                members: [],
                left: (f ? a.x: a) + 1,
                top: (f ? a.y: b) + 1,
                width: (f ? a.width: c) - 1,
                height: (f ? a.height: d) - 1,
                getCSS: function(a) {
                    var b = a.element,
                    c = b.nodeName,
                    a = a.inverted,
                    d = this.top - (c === "shape" ? b.offsetTop: 0),
                    e = this.left,
                    b = e + this.width,
                    f = d + this.height,
                    d = {
                        clip: "rect(" + s(a ? e: d) + "px," + s(a ? f: b) + "px," + s(a ? b: f) + "px," + s(a ? d: e) + "px)"
                    }; ! a && lb && c === "DIV" && v(d, {
                        width: b + "px",
                        height: f + "px"
                    });
                    return d
                },
                updateClipping: function() {
                    q(e.members,
                    function(a) {
                        a.element && a.css(e.getCSS(a))
                    })
                }
            })
        },
        color: function(a, b, c, d) {
            var e = this,
            f, g = /^rgba/,
            h, i, k = Y;
            a && a.linearGradient ? i = "gradient": a && a.radialGradient && (i = "pattern");
            if (i) {
                var j, l, m = a.linearGradient || a.radialGradient,
                n, p, u, o, G, x = "",
                a = a.stops,
                r, t = [],
                s = function() {
                    h = ['<fill colors="' + t.join(",") + '" opacity="', u, '" o:opacity2="', p, '" type="', i, '" ', x, 'focus="100%" method="any" />'];
                    $(e.prepVML(h), null, null, b)
                };
                n = a[0];
                r = a[a.length - 1];
                n[0] > 0 && a.unshift([0, n[1]]);
                r[0] < 1 && a.push([1, r[1]]);
                q(a,
                function(a, b) {
                    g.test(a[1]) ? (f = Fa(a[1]), j = f.get("rgb"), l = f.get("a")) : (j = a[1], l = 1);
                    t.push(a[0] * 100 + "% " + j);
                    b ? (u = l, o = j) : (p = l, G = j)
                });
                if (c === "fill") if (i === "gradient") c = m.x1 || m[0] || 0,
                a = m.y1 || m[1] || 0,
                n = m.x2 || m[2] || 0,
                m = m.y2 || m[3] || 0,
                x = 'angle="' + (90 - V.atan((m - a) / (n - c)) * 180 / ra) + '"',
                s();
                else {
                    var k = m.r,
                    w = k * 2,
                    v = k * 2,
                    y = m.cx,
                    D = m.cy,
                    U = b.radialReference,
                    z, k = function() {
                        U && (z = d.getBBox(), y += (U[0] - z.x) / z.width - 0.5, D += (U[1] - z.y) / z.height - 0.5, w *= U[2] / z.width, v *= U[2] / z.height);
                        x = 'src="' + F.global.VMLRadialGradientURL + '" size="' + w + "," + v + '" origin="0.5,0.5" position="' + y + "," + D + '" color2="' + G + '" ';
                        s()
                    };
                    d.added ? k() : d.onAdd = k;
                    k = o
                } else k = j
            } else if (g.test(a) && b.tagName !== "IMG") f = Fa(a),
            h = ["<", c, ' opacity="', f.get("a"), '"/>'],
            $(this.prepVML(h), null, null, b),
            k = f.get("rgb");
            else {
                k = b.getElementsByTagName(c);
                if (k.length) k[0].opacity = 1,
                k[0].type = "solid";
                k = a
            }
            return k
        },
        prepVML: function(a) {
            var b = this.isIE8,
            a = a.join("");
            b ? (a = a.replace("/>", ' xmlns="urn:schemas-microsoft-com:vml" />'), a = a.indexOf('style="') === -1 ? a.replace("/>", ' style="display:inline-block;behavior:url(#default#VML);" />') : a.replace('style="', 'style="display:inline-block;behavior:url(#default#VML);')) : a = a.replace("<", "<hcv:");
            return a
        },
        text: ka.prototype.html,
        path: function(a) {
            var b = {
                coordsize: "10 10"
            };
            Pa(a) ? b.d = a: fa(a) && v(b, a);
            return this.createElement("shape").attr(b)
        },
        circle: function(a, b, c) {
            var d = this.symbol("circle");
            if (fa(a)) c = a.r,
            b = a.y,
            a = a.x;
            d.isCircle = !0;
            d.r = c;
            return d.attr({
                x: a,
                y: b
            })
        },
        g: function(a) {
            var b;
            a && (b = {
                className: "highcharts-" + a,
                "class": "highcharts-" + a
            });
            return this.createElement(Ta).attr(b)
        },
        image: function(a, b, c, d, e) {
            var f = this.createElement("img").attr({
                src: a
            });
            arguments.length > 1 && f.attr({
                x: b,
                y: c,
                width: d,
                height: e
            });
            return f
        },
        createElement: function(a) {
            return a === "rect" ? this.symbol(a) : ka.prototype.createElement.call(this, a)
        },
        invertChild: function(a, b) {
            var c = this,
            d = b.style,
            e = a.tagName === "IMG" && a.style;
            E(a, {
                flip: "x",
                left: I(d.width) - (e ? I(e.top) : 1),
                top: I(d.height) - (e ? I(e.left) : 1),
                rotation: -90
            });
            q(a.childNodes,
            function(b) {
                c.invertChild(b, a)
            })
        },
        symbols: {
            arc: function(a, b, c, d, e) {
                var f = e.start,
                g = e.end,
                h = e.r || c || d,
                c = e.innerR,
                d = da(f),
                i = ia(f),
                k = da(g),
                j = ia(g);
                if (g - f === 0) return ["x"];
                f = ["wa", a - h, b - h, a + h, b + h, a + h * d, b + h * i, a + h * k, b + h * j];
                e.open && !c && f.push("e", "M", a, b);
                f.push("at", a - c, b - c, a + c, b + c, a + c * k, b + c * j, a + c * d, b + c * i, "x", "e");
                f.isArc = !0;
                return f
            },
            circle: function(a, b, c, d, e) {
                e && (c = d = 2 * e.r);
                e && e.isCircle && (a -= c / 2, b -= d / 2);
                return ["wa", a, b, a + c, b + d, a + c, b + d / 2, a + c, b + d / 2, "e"]
            },
            rect: function(a, b, c, d, e) {
                return ka.prototype.symbols[!t(e) || !e.r ? "square": "callout"].call(0, a, b, c, d, e)
            }
        }
    },
    P.VMLRenderer = fb = function() {
        this.init.apply(this, arguments)
    },
    fb.prototype = y(ka.prototype, ea),
    Wa = fb;
    ka.prototype.measureSpanWidth = function(a, b) {
        var c = B.createElement("span"),
        d;
        d = B.createTextNode(a);
        c.appendChild(d);
        E(c, b);
        this.box.appendChild(c);
        d = c.offsetWidth;
        Sa(c);
        return d
    };
    var Vb;
    if (ja) P.CanVGRenderer = ea = function() {
        Ea = "http://www.w3.org/1999/xhtml"
    },
    ea.prototype.symbols = {},
    Vb = function() {
        function a() {
            var a = b.length,
            d;
            for (d = 0; d < a; d++) b[d]();
            b = []
        }
        var b = [];
        return {
            push: function(c, d) {
                b.length === 0 && ac(d, a);
                b.push(c)
            }
        }
    } (),
    Wa = ea;
    Za.prototype = {
        addLabel: function() {
            var a = this.axis,
            b = a.options,
            c = a.chart,
            d = a.horiz,
            e = a.categories,
            f = a.names,
            g = this.pos,
            h = b.labels,
            i = a.tickPositions,
            d = d && e && !h.step && !h.staggerLines && !h.rotation && c.plotWidth / i.length || !d && (c.margin[3] || c.chartWidth * 0.33),
            k = g === i[0],
            j = g === i[i.length - 1],
            l,
            f = e ? o(e[g], f[g], g) : g,
            e = this.label,
            m = i.info;
            a.isDatetimeAxis && m && (l = b.dateTimeLabelFormats[m.higherRanks[g] || m.unitName]);
            this.isFirst = k;
            this.isLast = j;
            b = a.labelFormatter.call({
                axis: a,
                chart: c,
                isFirst: k,
                isLast: j,
                dateTimeLabelFormat: l,
                value: a.isLog ? ha(oa(f)) : f
            });
            g = d && {
                width: w(1, s(d - 2 * (h.padding || 10))) + "px"
            };
            g = v(g, h.style);
            if (t(e)) e && e.attr({
                text: b
            }).css(g);
            else {
                l = {
                    align: a.labelAlign
                };
                if (la(h.rotation)) l.rotation = h.rotation;
                if (d && h.ellipsis) l._clipHeight = a.len / i.length;
                this.label = t(b) && h.enabled ? c.renderer.text(b, 0, 0, h.useHTML).attr(l).css(g).add(a.labelGroup) : null
            }
        },
        getLabelSize: function() {
            var a = this.label,
            b = this.axis;
            return a ? a.getBBox()[b.horiz ? "height": "width"] : 0
        },
        getLabelSides: function() {
            var a = this.label.getBBox(),
            b = this.axis,
            c = b.horiz,
            d = b.options.labels,
            a = c ? a.width: a.height,
            b = c ? d.x - a * {
                left: 0,
                center: 0.5,
                right: 1
            } [b.labelAlign] : 0;
            return [b, c ? a + b: a]
        },
        handleOverflow: function(a, b) {
            var c = !0,
            d = this.axis,
            e = this.isFirst,
            f = this.isLast,
            g = d.horiz ? b.x: b.y,
            h = d.reversed,
            i = d.tickPositions,
            k = this.getLabelSides(),
            j = k[0],
            k = k[1],
            l,
            m,
            n,
            p = this.label.line || 0;
            l = d.labelEdge;
            m = d.justifyLabels && (e || f);
            l[p] === r || g + j > l[p] ? l[p] = g + k: m || (c = !1);
            if (m) {
                l = (m = d.justifyToPlot) ? d.pos: 0;
                m = m ? l + d.len: d.chart.chartWidth;
                do a += e ? 1 : -1,
                n = d.ticks[i[a]];
                while (i[a] && (!n || n.label.line !== p));
                d = n && n.label.xy && n.label.xy.x + n.getLabelSides()[e ? 0 : 1];
                e && !h || f && h ? g + j < l && (g = l - j, n && g + k > d && (c = !1)) : g + k > m && (g = m - k, n && g + j < d && (c = !1));
                b.x = g
            }
            return c
        },
        getPosition: function(a, b, c, d) {
            var e = this.axis,
            f = e.chart,
            g = d && f.oldChartHeight || f.chartHeight;
            return {
                x: a ? e.translate(b + c, null, null, d) + e.transB: e.left + e.offset + (e.opposite ? (d && f.oldChartWidth || f.chartWidth) - e.right - e.left: 0),
                y: a ? g - e.bottom + e.offset - (e.opposite ? e.height: 0) : g - e.translate(b + c, null, null, d) - e.transB
            }
        },
        getLabelPosition: function(a, b, c, d, e, f, g, h) {
            var i = this.axis,
            k = i.transA,
            j = i.reversed,
            l = i.staggerLines,
            m = i.chart.renderer.fontMetrics(e.style.fontSize).b,
            n = e.rotation,
            a = a + e.x - (f && d ? f * k * (j ? -1 : 1) : 0),
            b = b + e.y - (f && !d ? f * k * (j ? 1 : -1) : 0);
            n && i.side === 2 && (b -= m - m * da(n * Ma)); ! t(e.y) && !n && (b += m - c.getBBox().height / 2);
            if (l) c.line = g / (h || 1) % l,
            b += c.line * (i.labelOffset / l);
            return {
                x: a,
                y: b
            }
        },
        getMarkPath: function(a, b, c, d, e, f) {
            return f.crispLine(["M", a, b, "L", a + (e ? 0 : -c), b + (e ? c: 0)], d)
        },
        render: function(a, b, c) {
            var d = this.axis,
            e = d.options,
            f = d.chart.renderer,
            g = d.horiz,
            h = this.type,
            i = this.label,
            k = this.pos,
            j = e.labels,
            l = this.gridLine,
            m = h ? h + "Grid": "grid",
            n = h ? h + "Tick": "tick",
            p = e[m + "LineWidth"],
            u = e[m + "LineColor"],
            q = e[m + "LineDashStyle"],
            G = e[n + "Length"],
            m = e[n + "Width"] || 0,
            x = e[n + "Color"],
            t = e[n + "Position"],
            n = this.mark,
            s = j.step,
            w = !0,
            v = d.tickmarkOffset,
            y = this.getPosition(g, k, v, b),
            z = y.x,
            y = y.y,
            D = g && z === d.pos + d.len || !g && y === d.pos ? -1 : 1;
            this.isActive = !0;
            if (p) {
                k = d.getPlotLinePath(k + v, p * D, b, !0);
                if (l === r) {
                    l = {
                        stroke: u,
                        "stroke-width": p
                    };
                    if (q) l.dashstyle = q;
                    if (!h) l.zIndex = 1;
                    if (b) l.opacity = 0;
                    this.gridLine = l = p ? f.path(k).attr(l).add(d.gridGroup) : null
                }
                if (!b && l && k) l[this.isNew ? "attr": "animate"]({
                    d: k,
                    opacity: c
                })
            }
            if (m && G) t === "inside" && (G = -G),
            d.opposite && (G = -G),
            h = this.getMarkPath(z, y, G, m * D, g, f),
            n ? n.animate({
                d: h,
                opacity: c
            }) : this.mark = f.path(h).attr({
                stroke: x,
                "stroke-width": m,
                opacity: c
            }).add(d.axisGroup);
            if (i && !isNaN(z)) i.xy = y = this.getLabelPosition(z, y, i, g, j, v, a, s),
            this.isFirst && !this.isLast && !o(e.showFirstLabel, 1) || this.isLast && !this.isFirst && !o(e.showLastLabel, 1) ? w = !1 : !d.isRadial && !j.step && !j.rotation && !b && c !== 0 && (w = this.handleOverflow(a, y)),
            s && a % s && (w = !1),
            w && !isNaN(y.y) ? (y.opacity = c, i[this.isNew ? "attr": "animate"](y), this.isNew = !1) : i.attr("y", -9999)
        },
        destroy: function() {
            Ka(this, this.axis)
        }
    };
    P.PlotLineOrBand = function(a, b) {
        this.axis = a;
        if (b) this.options = b,
        this.id = b.id
    };
    P.PlotLineOrBand.prototype = {
        render: function() {
            var a = this,
            b = a.axis,
            c = b.horiz,
            d = (b.pointRange || 0) / 2,
            e = a.options,
            f = e.label,
            g = a.label,
            h = e.width,
            i = e.to,
            k = e.from,
            j = t(k) && t(i),
            l = e.value,
            m = e.dashStyle,
            n = a.svgElem,
            p = [],
            u,
            q = e.color,
            G = e.zIndex,
            x = e.events,
            r = {},
            s = b.chart.renderer;
            b.isLog && (k = Ga(k), i = Ga(i), l = Ga(l));
            if (h) {
                if (p = b.getPlotLinePath(l, h), r = {
                    stroke: q,
                    "stroke-width": h
                },
                m) r.dashstyle = m
            } else if (j) {
                k = w(k, b.min - d);
                i = z(i, b.max + d);
                p = b.getPlotBandPath(k, i, e);
                if (q) r.fill = q;
                if (e.borderWidth) r.stroke = e.borderColor,
                r["stroke-width"] = e.borderWidth
            } else return;
            if (t(G)) r.zIndex = G;
            if (n) if (p) n.animate({
                d: p
            },
            null, n.onGetPath);
            else {
                if (n.hide(), n.onGetPath = function() {
                    n.show()
                },
                g) a.label = g = g.destroy()
            } else if (p && p.length && (a.svgElem = n = s.path(p).attr(r).add(), x)) for (u in d = function(b) {
                n.on(b,
                function(c) {
                    x[b].apply(a, [c])
                })
            },
            x) d(u);
            if (f && t(f.text) && p && p.length && b.width > 0 && b.height > 0) {
                f = y({
                    align: c && j && "center",
                    x: c ? !j && 4 : 10,
                    verticalAlign: !c && j && "middle",
                    y: c ? j ? 16 : 10 : j ? 6 : -4,
                    rotation: c && !j && 90
                },
                f);
                if (!g) {
                    r = {
                        align: f.textAlign || f.align,
                        rotation: f.rotation
                    };
                    if (t(G)) r.zIndex = G;
                    a.label = g = s.text(f.text, 0, 0, f.useHTML).attr(r).css(f.style).add()
                }
                b = [p[1], p[4], o(p[6], p[1])];
                p = [p[2], p[5], o(p[7], p[2])];
                c = Ra(b);
                j = Ra(p);
                g.align(f, !1, {
                    x: c,
                    y: j,
                    width: Ba(b) - c,
                    height: Ba(p) - j
                });
                g.show()
            } else g && g.hide();
            return a
        },
        destroy: function() {
            pa(this.axis.plotLinesAndBands, this);
            delete this.axis;
            Ka(this)
        }
    };
    L.prototype = {
        defaultOptions: {
            dateTimeLabelFormats: {
                millisecond: "%H:%M:%S.%L",
                second: "%H:%M:%S",
                minute: "%H:%M",
                hour: "%H:%M",
                day: "%e. %b",
                week: "%e. %b",
                month: "%b '%y",
                year: "%Y"
            },
            endOnTick: !1,
            gridLineColor: "#222",
            labels: Q,
            lineColor: "#666",
            lineWidth: 1,
            minPadding: 0.01,
            maxPadding: 0.01,
            minorGridLineColor: "#E0E0E0",
            minorGridLineWidth: 1,
            minorTickColor: "#A0A0A0",
            minorTickLength: 2,
            minorTickPosition: "outside",
            startOfWeek: 1,
            startOnTick: !1,
            tickColor: "#666",
            tickLength: 10,
            tickmarkPlacement: "between",
            tickPixelInterval: 100,
            tickPosition: "outside",
            tickWidth: 1,
            title: {
                align: "middle",
                style: {
                    color: "#707070"
                }
            },
            type: "linear"
        },
        defaultYAxisOptions: {
            endOnTick: !0,
            gridLineWidth: 1,
            tickPixelInterval: 72,
            showLastLabel: !0,
            labels: {
                x: -8,
                y: 3
            },
            lineWidth: 0,
            maxPadding: 0.05,
            minPadding: 0.05,
            startOnTick: !0,
            tickWidth: 0,
            title: {
                rotation: 270,
                text: "Values"
            },
            stackLabels: {
                enabled: !1,
                formatter: function() {
                    return Ia(this.total, -1)
                },
                style: Q.style
            }
        },
        defaultLeftAxisOptions: {
            labels: {
                x: -15,
                y: null
            },
            title: {
                rotation: 270
            }
        },
        defaultRightAxisOptions: {
            labels: {
                x: 15,
                y: null
            },
            title: {
                rotation: 90
            }
        },
        defaultBottomAxisOptions: {
            labels: {
                x: 0,
                y: 20
            },
            title: {
                rotation: 0
            }
        },
        defaultTopAxisOptions: {
            labels: {
                x: 0,
                y: -15
            },
            title: {
                rotation: 0
            }
        },
        init: function(a, b) {
            var c = b.isX;
            this.horiz = a.inverted ? !c: c;
            this.coll = (this.isXAxis = c) ? "xAxis": "yAxis";
            this.opposite = b.opposite;
            this.side = b.side || (this.horiz ? this.opposite ? 0 : 2 : this.opposite ? 1 : 3);
            this.setOptions(b);
            var d = this.options,
            e = d.type;
            this.labelFormatter = d.labels.formatter || this.defaultLabelFormatter;
            this.userOptions = b;
            this.minPixelPadding = 0;
            this.chart = a;
            this.reversed = d.reversed;
            this.zoomEnabled = d.zoomEnabled !== !1;
            this.categories = d.categories || e === "category";
            this.names = [];
            this.isLog = e === "logarithmic";
            this.isDatetimeAxis = e === "datetime";
            this.isLinked = t(d.linkedTo);
            this.tickmarkOffset = this.categories && d.tickmarkPlacement === "between" ? 0.5 : 0;
            this.ticks = {};
            this.labelEdge = [];
            this.minorTicks = {};
            this.plotLinesAndBands = [];
            this.alternateBands = {};
            this.len = 0;
            this.minRange = this.userMinRange = d.minRange || d.maxZoom;
            this.range = d.range;
            this.offset = d.offset || 0;
            this.stacks = {};
            this.oldStacks = {};
            this.min = this.max = null;
            this.crosshair = o(d.crosshair, ma(a.options.tooltip.crosshairs)[c ? 0 : 1], !1);
            var f, d = this.options.events;
            wa(this, a.axes) === -1 && (c && !this.isColorAxis ? a.axes.splice(a.xAxis.length, 0, this) : a.axes.push(this), a[this.coll].push(this));
            this.series = this.series || [];
            if (a.inverted && c && this.reversed === r) this.reversed = !0;
            this.removePlotLine = this.removePlotBand = this.removePlotBandOrLine;
            for (f in d) A(this, f, d[f]);
            if (this.isLog) this.val2lin = Ga,
            this.lin2val = oa
        },
        setOptions: function(a) {
            this.options = y(this.defaultOptions, this.isXAxis ? {}: this.defaultYAxisOptions, [this.defaultTopAxisOptions, this.defaultRightAxisOptions, this.defaultBottomAxisOptions, this.defaultLeftAxisOptions][this.side], y(F[this.coll], a))
        },
        defaultLabelFormatter: function() {
            var a = this.axis,
            b = this.value,
            c = a.categories,
            d = this.dateTimeLabelFormat,
            e = F.lang.numericSymbols,
            f = e && e.length,
            g, h = a.options.labels.format,
            a = a.isLog ? b: a.tickInterval;
            if (h) g = Ja(h, this);
            else if (c) g = b;
            else if (d) g = ua(d, b);
            else if (f && a >= 1E3) for (; f--&&g === r;) c = Math.pow(1E3, f + 1),
            a >= c && e[f] !== null && (g = Ia(b / c, -1) + e[f]);
            g === r && (g = M(b) >= 1E4 ? Ia(b, 0) : Ia(b, -1, r, ""));
            return g
        },
        getSeriesExtremes: function() {
            var a = this,
            b = a.chart;
            a.hasVisibleSeries = !1;
            a.dataMin = a.dataMax = null;
            a.buildStacks && a.buildStacks();
            q(a.series,
            function(c) {
                if (c.visible || !b.options.chart.ignoreHiddenSeries) {
                    var d;
                    d = c.options.threshold;
                    var e;
                    a.hasVisibleSeries = !0;
                    a.isLog && d <= 0 && (d = null);
                    if (a.isXAxis) {
                        if (d = c.xData, d.length) a.dataMin = z(o(a.dataMin, d[0]), Ra(d)),
                        a.dataMax = w(o(a.dataMax, d[0]), Ba(d))
                    } else {
                        c.getExtremes();
                        e = c.dataMax;
                        c = c.dataMin;
                        if (t(c) && t(e)) a.dataMin = z(o(a.dataMin, c), c),
                        a.dataMax = w(o(a.dataMax, e), e);
                        if (t(d)) if (a.dataMin >= d) a.dataMin = d,
                        a.ignoreMinPadding = !0;
                        else if (a.dataMax < d) a.dataMax = d,
                        a.ignoreMaxPadding = !0
                    }
                }
            })
        },
        translate: function(a, b, c, d, e, f) {
            var g = 1,
            h = 0,
            i = d ? this.oldTransA: this.transA,
            d = d ? this.oldMin: this.min,
            k = this.minPixelPadding,
            e = (this.options.ordinal || this.isLog && e) && this.lin2val;
            if (!i) i = this.transA;
            if (c) g *= -1,
            h = this.len;
            this.reversed && (g *= -1, h -= g * (this.sector || this.len));
            b ? (a = a * g + h, a -= k, a = a / i + d, e && (a = this.lin2val(a))) : (e && (a = this.val2lin(a)), f === "between" && (f = 0.5), a = g * (a - d) * i + h + g * k + (la(f) ? i * f * this.pointRange: 0));
            return a
        },
        toPixels: function(a, b) {
            return this.translate(a, !1, !this.horiz, null, !0) + (b ? 0 : this.pos)
        },
        toValue: function(a, b) {
            return this.translate(a - (b ? 0 : this.pos), !0, !this.horiz, null, !0)
        },
        getPlotLinePath: function(a, b, c, d, e) {
            var f = this.chart,
            g = this.left,
            h = this.top,
            i, k, j = c && f.oldChartHeight || f.chartHeight,
            l = c && f.oldChartWidth || f.chartWidth,
            m;
            i = this.transB;
            e = o(e, this.translate(a, null, null, c));
            a = c = s(e + i);
            i = k = s(j - e - i);
            if (isNaN(e)) m = !0;
            else if (this.horiz) {
                if (i = h, k = j - this.bottom, a < g || a > g + this.width) m = !0
            } else if (a = g, c = l - this.right, i < h || i > h + this.height) m = !0;
            return m && !d ? null: f.renderer.crispLine(["M", a, i, "L", c, k], b || 1)
        },
        getLinearTickPositions: function(a, b, c) {
            var d, e = ha(S(b / a) * a),
            f = ha(Va(c / a) * a),
            g = [];
            if (b === c && la(b)) return [b];
            for (b = e; b <= f;) {
                g.push(b);
                b = ha(b + a);
                if (b === d) break;
                d = b
            }
            return g
        },
        getMinorTickPositions: function() {
            var a = this.options,
            b = this.tickPositions,
            c = this.minorTickInterval,
            d = [],
            e;
            if (this.isLog) {
                e = b.length;
                for (a = 1; a < e; a++) d = d.concat(this.getLogTickPositions(c, b[a - 1], b[a], !0))
            } else if (this.isDatetimeAxis && a.minorTickInterval === "auto") d = d.concat(this.getTimeTicks(this.normalizeTimeTickInterval(c), this.min, this.max, a.startOfWeek)),
            d[0] < this.min && d.shift();
            else for (b = this.min + (b[0] - this.min) % c; b <= this.max; b += c) d.push(b);
            return d
        },
        adjustForMinRange: function() {
            var a = this.options,
            b = this.min,
            c = this.max,
            d, e = this.dataMax - this.dataMin >= this.minRange,
            f, g, h, i, k;
            if (this.isXAxis && this.minRange === r && !this.isLog) t(a.min) || t(a.max) ? this.minRange = null: (q(this.series,
            function(a) {
                i = a.xData;
                for (g = k = a.xIncrement ? 1 : i.length - 1; g > 0; g--) if (h = i[g] - i[g - 1], f === r || h < f) f = h
            }), this.minRange = z(f * 5, this.dataMax - this.dataMin));
            if (c - b < this.minRange) {
                var j = this.minRange;
                d = (j - c + b) / 2;
                d = [b - d, o(a.min, b - d)];
                if (e) d[2] = this.dataMin;
                b = Ba(d);
                c = [b + j, o(a.max, b + j)];
                if (e) c[2] = this.dataMax;
                c = Ra(c);
                c - b < j && (d[0] = c - j, d[1] = o(a.min, c - j), b = Ba(d))
            }
            this.min = b;
            this.max = c
        },
        setAxisTranslation: function(a) {
            var b = this,
            c = b.max - b.min,
            d = b.axisPointRange || 0,
            e, f = 0,
            g = 0,
            h = b.linkedParent,
            i = !!b.categories,
            k = b.transA;
            if (b.isXAxis || i || d) h ? (f = h.minPointOffset, g = h.pointRangePadding) : q(b.series,
            function(a) {
                var h = i ? 1 : b.isXAxis ? a.pointRange: b.axisPointRange || 0,
                k = a.options.pointPlacement,
                n = a.closestPointRange;
                h > c && (h = 0);
                d = w(d, h);
                f = w(f, Oa(k) ? 0 : h / 2);
                g = w(g, k === "on" ? 0 : h); ! a.noSharedTooltip && t(n) && (e = t(e) ? z(e, n) : n)
            }),
            h = b.ordinalSlope && e ? b.ordinalSlope / e: 1,
            b.minPointOffset = f *= h,
            b.pointRangePadding = g *= h,
            b.pointRange = z(d, c),
            b.closestPointRange = e;
            if (a) b.oldTransA = k;
            b.translationSlope = b.transA = k = b.len / (c + g || 1);
            b.transB = b.horiz ? b.left: b.bottom;
            b.minPixelPadding = k * f
        },
        setTickPositions: function(a) {
            var b = this,
            c = b.chart,
            d = b.options,
            e = b.isLog,
            f = b.isDatetimeAxis,
            g = b.isXAxis,
            h = b.isLinked,
            i = b.options.tickPositioner,
            k = d.maxPadding,
            j = d.minPadding,
            l = d.tickInterval,
            m = d.minTickInterval,
            n = d.tickPixelInterval,
            p, u = b.categories;
            h ? (b.linkedParent = c[b.coll][d.linkedTo], c = b.linkedParent.getExtremes(), b.min = o(c.min, c.dataMin), b.max = o(c.max, c.dataMax), d.type !== b.linkedParent.options.type && qa(11, 1)) : (b.min = o(b.userMin, d.min, b.dataMin), b.max = o(b.userMax, d.max, b.dataMax));
            if (e) ! a && z(b.min, o(b.dataMin, b.min)) <= 0 && qa(10, 1),
            b.min = ha(Ga(b.min)),
            b.max = ha(Ga(b.max));
            if (b.range && t(b.max)) b.userMin = b.min = w(b.min, b.max - b.range),
            b.userMax = b.max,
            b.range = null;
            b.beforePadding && b.beforePadding();
            b.adjustForMinRange();
            if (!u && !b.axisPointRange && !b.usePercentage && !h && t(b.min) && t(b.max) && (c = b.max - b.min)) {
                if (!t(d.min) && !t(b.userMin) && j && (b.dataMin < 0 || !b.ignoreMinPadding)) b.min -= c * j;
                if (!t(d.max) && !t(b.userMax) && k && (b.dataMax > 0 || !b.ignoreMaxPadding)) b.max += c * k
            }
            if (la(d.floor)) b.min = w(b.min, d.floor);
            if (la(d.ceiling)) b.max = z(b.max, d.ceiling);
            b.min === b.max || b.min === void 0 || b.max === void 0 ? b.tickInterval = 1 : h && !l && n === b.linkedParent.options.tickPixelInterval ? b.tickInterval = b.linkedParent.tickInterval: (b.tickInterval = o(l, u ? 1 : (b.max - b.min) * n / w(b.len, n)), !t(l) && b.len < n && !this.isRadial && !this.isLog && !u && d.startOnTick && d.endOnTick && (p = !0, b.tickInterval /= 4));
            g && !a && q(b.series,
            function(a) {
                a.processData(b.min !== b.oldMin || b.max !== b.oldMax)
            });
            b.setAxisTranslation(!0);
            b.beforeSetTickPositions && b.beforeSetTickPositions();
            if (b.postProcessTickInterval) b.tickInterval = b.postProcessTickInterval(b.tickInterval);
            if (b.pointRange) b.tickInterval = w(b.pointRange, b.tickInterval);
            if (!l && b.tickInterval < m) b.tickInterval = m;
            if (!f && !e && !l) b.tickInterval = sb(b.tickInterval, null, rb(b.tickInterval), d);
            b.minorTickInterval = d.minorTickInterval === "auto" && b.tickInterval ? b.tickInterval / 5 : d.minorTickInterval;
            b.tickPositions = a = d.tickPositions ? [].concat(d.tickPositions) : i && i.apply(b, [b.min, b.max]);
            if (!a) ! b.ordinalPositions && (b.max - b.min) / b.tickInterval > w(2 * b.len, 200) && qa(19, !0),
            a = f ? b.getTimeTicks(b.normalizeTimeTickInterval(b.tickInterval, d.units), b.min, b.max, d.startOfWeek, b.ordinalPositions, b.closestPointRange, !0) : e ? b.getLogTickPositions(b.tickInterval, b.min, b.max) : b.getLinearTickPositions(b.tickInterval, b.min, b.max),
            p && a.splice(1, a.length - 2),
            b.tickPositions = a;
            if (!h) e = a[0],
            f = a[a.length - 1],
            h = b.minPointOffset || 0,
            d.startOnTick ? b.min = e: b.min - h > e && a.shift(),
            d.endOnTick ? b.max = f: b.max + h < f && a.pop(),
            a.length === 1 && (d = M(b.max) > 1E13 ? 1 : 0.001, b.min -= d, b.max += d)
        },
        setMaxTicks: function() {
            var a = this.chart,
            b = a.maxTicks || {},
            c = this.tickPositions,
            d = this._maxTicksKey = [this.coll, this.pos, this.len].join("-");
            if (!this.isLinked && !this.isDatetimeAxis && c && c.length > (b[d] || 0) && this.options.alignTicks !== !1) b[d] = c.length;
            a.maxTicks = b
        },
        adjustTickAmount: function() {
            var a = this._maxTicksKey,
            b = this.tickPositions,
            c = this.chart.maxTicks;
            if (c && c[a] && !this.isDatetimeAxis && !this.categories && !this.isLinked && this.options.alignTicks !== !1 && this.min !== r) {
                var d = this.tickAmount,
                e = b.length;
                this.tickAmount = a = c[a];
                if (e < a) {
                    for (; b.length < a;) b.push(ha(b[b.length - 1] + this.tickInterval));
                    this.transA *= (e - 1) / (a - 1);
                    this.max = b[b.length - 1]
                }
                if (t(d) && a !== d) this.isDirty = !0
            }
        },
        setScale: function() {
            var a = this.stacks,
            b, c, d, e;
            this.oldMin = this.min;
            this.oldMax = this.max;
            this.oldAxisLength = this.len;
            this.setAxisSize();
            e = this.len !== this.oldAxisLength;
            q(this.series,
            function(a) {
                if (a.isDirtyData || a.isDirty || a.xAxis.isDirty) d = !0
            });
            if (e || d || this.isLinked || this.forceRedraw || this.userMin !== this.oldUserMin || this.userMax !== this.oldUserMax) {
                if (!this.isXAxis) for (b in a) for (c in a[b]) a[b][c].total = null,
                a[b][c].cum = 0;
                this.forceRedraw = !1;
                this.getSeriesExtremes();
                this.setTickPositions();
                this.oldUserMin = this.userMin;
                this.oldUserMax = this.userMax;
                if (!this.isDirty) this.isDirty = e || this.min !== this.oldMin || this.max !== this.oldMax
            } else if (!this.isXAxis) {
                if (this.oldStacks) a = this.stacks = this.oldStacks;
                for (b in a) for (c in a[b]) a[b][c].cum = a[b][c].total
            }
            this.setMaxTicks()
        },
        setExtremes: function(a, b, c, d, e) {
            var f = this,
            g = f.chart,
            c = o(c, !0),
            e = v(e, {
                min: a,
                max: b
            });
            N(f, "setExtremes", e,
            function() {
                f.userMin = a;
                f.userMax = b;
                f.eventArgs = e;
                f.isDirtyExtremes = !0;
                c && g.redraw(d)
            })
        },
        zoom: function(a, b) {
            var c = this.dataMin,
            d = this.dataMax,
            e = this.options;
            this.allowZoomOutside || (t(c) && a <= z(c, o(e.min, c)) && (a = r), t(d) && b >= w(d, o(e.max, d)) && (b = r));
            this.displayBtn = a !== r || b !== r;
            this.setExtremes(a, b, !1, r, {
                trigger: "zoom"
            });
            return ! 0
        },
        setAxisSize: function() {
            var a = this.chart,
            b = this.options,
            c = b.offsetLeft || 0,
            d = this.horiz,
            e = o(b.width, a.plotWidth - c + (b.offsetRight || 0)),
            f = o(b.height, a.plotHeight),
            g = o(b.top, a.plotTop),
            b = o(b.left, a.plotLeft + c),
            c = /%$/;
            c.test(f) && (f = parseInt(f, 10) / 100 * a.plotHeight);
            c.test(g) && (g = parseInt(g, 10) / 100 * a.plotHeight + a.plotTop);
            this.left = b;
            this.top = g;
            this.width = e;
            this.height = f;
            this.bottom = a.chartHeight - f - g;
            this.right = a.chartWidth - e - b;
            this.len = w(d ? e: f, 0);
            this.pos = d ? b: g
        },
        getExtremes: function() {
            var a = this.isLog;
            return {
                min: a ? ha(oa(this.min)) : this.min,
                max: a ? ha(oa(this.max)) : this.max,
                dataMin: this.dataMin,
                dataMax: this.dataMax,
                userMin: this.userMin,
                userMax: this.userMax
            }
        },
        getThreshold: function(a) {
            var b = this.isLog,
            c = b ? oa(this.min) : this.min,
            b = b ? oa(this.max) : this.max;
            c > a || a === null ? a = c: b < a && (a = b);
            return this.translate(a, 0, 1, 0, 1)
        },
        autoLabelAlign: function(a) {
            a = (o(a, 0) - this.side * 90 + 720) % 360;
            return a > 15 && a < 165 ? "right": a > 195 && a < 345 ? "left": "center"
        },
        getOffset: function() {
            var a = this,
            b = a.chart,
            c = b.renderer,
            d = a.options,
            e = a.tickPositions,
            f = a.ticks,
            g = a.horiz,
            h = a.side,
            i = b.inverted ? [1, 0, 3, 2][h] : h,
            k,
            j = 0,
            l,
            m = 0,
            n = d.title,
            p = d.labels,
            u = 0,
            Db = b.axisOffset,
            G = b.clipOffset,
            x = [ - 1, 1, 1, -1][h],
            s,
            v = 1,
            y = o(p.maxStaggerLines, 5),
            z,
            A,
            B,
            D,
            U = h === 2 ? c.fontMetrics(p.style.fontSize).b: 0;
            a.hasData = k = a.hasVisibleSeries || t(a.min) && t(a.max) && !!e;
            a.showAxis = b = k || o(d.showEmpty, !0);
            a.staggerLines = a.horiz && p.staggerLines;
            if (!a.axisGroup) a.gridGroup = c.g("grid").attr({
                zIndex: d.gridZIndex || 1
            }).add(),
            a.axisGroup = c.g("axis").attr({
                zIndex: d.zIndex || 2
            }).add(),
            a.labelGroup = c.g("axis-labels").attr({
                zIndex: p.zIndex || 7
            }).addClass("highcharts-" + a.coll.toLowerCase() + "-labels").add();
            if (k || a.isLinked) {
                a.labelAlign = o(p.align || a.autoLabelAlign(p.rotation));
                q(e,
                function(b) {
                    f[b] ? f[b].addLabel() : f[b] = new Za(a, b)
                });
                if (a.horiz && !a.staggerLines && y && !p.rotation) {
                    for (s = a.reversed ? [].concat(e).reverse() : e; v < y;) {
                        k = [];
                        z = !1;
                        for (p = 0; p < s.length; p++) A = s[p],
                        B = (B = f[A].label && f[A].label.getBBox()) ? B.width: 0,
                        D = p % v,
                        B && (A = a.translate(A), k[D] !== r && A < k[D] && (z = !0), k[D] = A + B);
                        if (z) v++;
                        else break
                    }
                    if (v > 1) a.staggerLines = v
                }
                q(e,
                function(b) {
                    if (h === 0 || h === 2 || {
                        1 : "left",
                        3 : "right"
                    } [h] === a.labelAlign) u = w(f[b].getLabelSize(), u)
                });
                if (a.staggerLines) u *= a.staggerLines,
                a.labelOffset = u
            } else for (s in f) f[s].destroy(),
            delete f[s];
            if (n && n.text && n.enabled !== !1) {
                if (!a.axisTitle) a.axisTitle = c.text(n.text, 0, 0, n.useHTML).attr({
                    zIndex: 7,
                    rotation: n.rotation || 0,
                    align: n.textAlign || {
                        low: "left",
                        middle: "center",
                        high: "right"
                    } [n.align]
                }).addClass("highcharts-" + this.coll.toLowerCase() + "-title").css(n.style).add(a.axisGroup),
                a.axisTitle.isNew = !0;
                if (b) j = a.axisTitle.getBBox()[g ? "height": "width"],
                m = o(n.margin, g ? 5 : 10),
                l = n.offset;
                a.axisTitle[b ? "show": "hide"]()
            }
            a.offset = x * o(d.offset, Db[h]);
            a.axisTitleMargin = o(l, u + m + (u && x * d.labels[g ? "y": "x"] - U));
            Db[h] = w(Db[h], a.axisTitleMargin + j + x * a.offset);
            G[i] = w(G[i], S(d.lineWidth / 2) * 2)
        },
        getLinePath: function(a) {
            var b = this.chart,
            c = this.opposite,
            d = this.offset,
            e = this.horiz,
            f = this.left + (c ? this.width: 0) + d,
            d = b.chartHeight - this.bottom - (c ? this.height: 0) + d;
            c && (a *= -1);
            return b.renderer.crispLine(["M", e ? this.left: f, e ? d: this.top, "L", e ? b.chartWidth - this.right: f, e ? d: b.chartHeight - this.bottom], a)
        },
        getTitlePosition: function() {
            var a = this.horiz,
            b = this.left,
            c = this.top,
            d = this.len,
            e = this.options.title,
            f = a ? b: c,
            g = this.opposite,
            h = this.offset,
            i = I(e.style.fontSize || 12),
            d = {
                low: f + (a ? 0 : d),
                middle: f + d / 2,
                high: f + (a ? d: 0)
            } [e.align],
            b = (a ? c + this.height: b) + (a ? 1 : -1) * (g ? -1 : 1) * this.axisTitleMargin + (this.side === 2 ? i: 0);
            return {
                x: a ? d: b + (g ? this.width: 0) + h + (e.x || 0),
                y: a ? b - (g ? this.height: 0) + h: d + (e.y || 0)
            }
        },
        render: function() {
            var a = this,
            b = a.horiz,
            c = a.reversed,
            d = a.chart,
            e = d.renderer,
            f = a.options,
            g = a.isLog,
            h = a.isLinked,
            i = a.tickPositions,
            k, j = a.axisTitle,
            l = a.ticks,
            m = a.minorTicks,
            n = a.alternateBands,
            p = f.stackLabels,
            u = f.alternateGridColor,
            o = a.tickmarkOffset,
            G = f.lineWidth,
            x = d.hasRendered && t(a.oldMin) && !isNaN(a.oldMin),
            s = a.hasData,
            w = a.showAxis,
            v,
            y = f.labels.overflow,
            z = a.justifyLabels = b && y !== !1,
            A;
            a.labelEdge.length = 0;
            a.justifyToPlot = y === "justify";
            q([l, m, n],
            function(a) {
                for (var b in a) a[b].isActive = !1
            });
            if (s || h) if (a.minorTickInterval && !a.categories && q(a.getMinorTickPositions(),
            function(b) {
                m[b] || (m[b] = new Za(a, b, "minor"));
                x && m[b].isNew && m[b].render(null, !0);
                m[b].render(null, !1, 1)
            }), i.length && (k = i.slice(), (b && c || !b && !c) && k.reverse(), z && (k = k.slice(1).concat([k[0]])), q(k,
            function(b, c) {
                z && (c = c === k.length - 1 ? 0 : c + 1);
                if (!h || b >= a.min && b <= a.max) l[b] || (l[b] = new Za(a, b)),
                x && l[b].isNew && l[b].render(c, !0, 0.1),
                l[b].render(c, !1, 1)
            }), o && a.min === 0 && (l[ - 1] || (l[ - 1] = new Za(a, -1, null, !0)), l[ - 1].render( - 1))), u && q(i,
            function(b, c) {
                if (c % 2 === 0 && b < a.max) n[b] || (n[b] = new P.PlotLineOrBand(a)),
                v = b + o,
                A = i[c + 1] !== r ? i[c + 1] + o: a.max,
                n[b].options = {
                    from: g ? oa(v) : v,
                    to: g ? oa(A) : A,
                    color: u
                },
                n[b].render(),
                n[b].isActive = !0
            }), !a._addedPlotLB) q((f.plotLines || []).concat(f.plotBands || []),
            function(b) {
                a.addPlotBandOrLine(b)
            }),
            a._addedPlotLB = !0;
            q([l, m, n],
            function(a) {
                var b, c, e = [],
                f = Ca ? Ca.duration || 500 : 0,
                g = function() {
                    for (c = e.length; c--;) a[e[c]] && !a[e[c]].isActive && (a[e[c]].destroy(), delete a[e[c]])
                };
                for (b in a) if (!a[b].isActive) a[b].render(b, !1, 0),
                a[b].isActive = !1,
                e.push(b);
                a === n || !d.hasRendered || !f ? g() : f && setTimeout(g, f)
            });
            if (G) b = a.getLinePath(G),
            a.axisLine ? a.axisLine.animate({
                d: b
            }) : a.axisLine = e.path(b).attr({
                stroke: f.lineColor,
                "stroke-width": G,
                zIndex: 7
            }).add(a.axisGroup),
            a.axisLine[w ? "show": "hide"]();
            if (j && w) j[j.isNew ? "attr": "animate"](a.getTitlePosition()),
            j.isNew = !1;
            p && p.enabled && a.renderStackTotals();
            a.isDirty = !1
        },
        redraw: function() {
            var a = this.chart.pointer;
            a && a.reset(!0);
            this.render();
            q(this.plotLinesAndBands,
            function(a) {
                a.render()
            });
            q(this.series,
            function(a) {
                a.isDirty = !0
            })
        },
        destroy: function(a) {
            var b = this,
            c = b.stacks,
            d, e = b.plotLinesAndBands;
            a || R(b);
            for (d in c) Ka(c[d]),
            c[d] = null;
            q([b.ticks, b.minorTicks, b.alternateBands],
            function(a) {
                Ka(a)
            });
            for (a = e.length; a--;) e[a].destroy();
            q("stackTotalGroup,axisLine,axisTitle,axisGroup,cross,gridGroup,labelGroup".split(","),
            function(a) {
                b[a] && (b[a] = b[a].destroy())
            });
            this.cross && this.cross.destroy()
        },
        drawCrosshair: function(a, b) {
            if (this.crosshair) if ((t(b) || !o(this.crosshair.snap, !0)) === !1) this.hideCrosshair();
            else {
                var c, d = this.crosshair,
                e = d.animation;
                o(d.snap, !0) ? t(b) && (c = this.chart.inverted != this.horiz ? b.plotX: this.len - b.plotY) : c = this.horiz ? a.chartX - this.pos: this.len - a.chartY + this.pos;
                c = this.isRadial ? this.getPlotLinePath(this.isXAxis ? b.x: o(b.stackY, b.y)) : this.getPlotLinePath(null, null, null, null, c);
                if (c === null) this.hideCrosshair();
                else if (this.cross) this.cross.attr({
                    visibility: "visible"
                })[e ? "animate": "attr"]({
                    d: c
                },
                e);
                else {
                    e = {
                        "stroke-width": d.width || 1,
                        stroke: d.color || "#C0C0C0",
                        zIndex: d.zIndex || 2
                    };
                    if (d.dashStyle) e.dashstyle = d.dashStyle;
                    this.cross = this.chart.renderer.path(c).attr(e).add()
                }
            }
        },
        hideCrosshair: function() {
            this.cross && this.cross.hide()
        }
    };
    v(L.prototype, {
        getPlotBandPath: function(a, b) {
            var c = this.getPlotLinePath(b),
            d = this.getPlotLinePath(a);
            d && c ? d.push(c[4], c[5], c[1], c[2]) : d = null;
            return d
        },
        addPlotBand: function(a) {
            this.addPlotBandOrLine(a, "plotBands")
        },
        addPlotLine: function(a) {
            this.addPlotBandOrLine(a, "plotLines")
        },
        addPlotBandOrLine: function(a, b) {
            var c = (new P.PlotLineOrBand(this, a)).render(),
            d = this.userOptions;
            c && (b && (d[b] = d[b] || [], d[b].push(a)), this.plotLinesAndBands.push(c));
            return c
        },
        removePlotBandOrLine: function(a) {
            for (var b = this.plotLinesAndBands,
            c = this.options,
            d = this.userOptions,
            e = b.length; e--;) b[e].id === a && b[e].destroy();
            q([c.plotLines || [], d.plotLines || [], c.plotBands || [], d.plotBands || []],
            function(b) {
                for (e = b.length; e--;) b[e].id === a && pa(b, b[e])
            })
        }
    });
    L.prototype.getTimeTicks = function(a, b, c, d) {
        var e = [],
        f = {},
        g = F.global.useUTC,
        h,
        i = new Date(b - La),
        k = a.unitRange,
        j = a.count;
        if (t(b)) {
            k >= H.second && (i.setMilliseconds(0), i.setSeconds(k >= H.minute ? 0 : j * S(i.getSeconds() / j)));
            if (k >= H.minute) i[Nb](k >= H.hour ? 0 : j * S(i[ub]() / j));
            if (k >= H.hour) i[Ob](k >= H.day ? 0 : j * S(i[vb]() / j));
            if (k >= H.day) i[xb](k >= H.month ? 1 : j * S(i[Ua]() / j));
            k >= H.month && (i[Pb](k >= H.year ? 0 : j * S(i[jb]() / j)), h = i[kb]());
            k >= H.year && (h -= h % j, i[Qb](h));
            if (k === H.week) i[xb](i[Ua]() - i[wb]() + o(d, 1));
            b = 1;
            La && (i = new Date(i.getTime() + La));
            h = i[kb]();
            for (var d = i.getTime(), l = i[jb](), m = i[Ua](), n = g ? La: (864E5 + i.getTimezoneOffset() * 6E4) % 864E5; d < c;) e.push(d),
            k === H.year ? d = ib(h + b * j, 0) : k === H.month ? d = ib(h, l + b * j) : !g && (k === H.day || k === H.week) ? d = ib(h, l, m + b * j * (k === H.day ? 1 : 7)) : d += k * j,
            b++;
            e.push(d);
            q(Cb(e,
            function(a) {
                return k <= H.hour && a % H.day === n
            }),
            function(a) {
                f[a] = "day"
            })
        }
        e.info = v(a, {
            higherRanks: f,
            totalRange: k * j
        });
        return e
    };
    L.prototype.normalizeTimeTickInterval = function(a, b) {
        var c = b || [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1, 2]], ["week", [1, 2]], ["month", [1, 2, 3, 4, 6]], ["year", null]],
        d = c[c.length - 1],
        e = H[d[0]],
        f = d[1],
        g;
        for (g = 0; g < c.length; g++) if (d = c[g], e = H[d[0]], f = d[1], c[g + 1] && a <= (e * f[f.length - 1] + H[c[g + 1][0]]) / 2) break;
        e === H.year && a < 5 * e && (f = [1, 2, 5]);
        c = sb(a / e, f, d[0] === "year" ? w(rb(a / e), 1) : 1);
        return {
            unitRange: e,
            count: c,
            unitName: d[0]
        }
    };
    L.prototype.getLogTickPositions = function(a, b, c, d) {
        var e = this.options,
        f = this.len,
        g = [];
        if (!d) this._minorAutoInterval = null;
        if (a >= 0.5) a = s(a),
        g = this.getLinearTickPositions(a, b, c);
        else if (a >= 0.08) for (var f = S(b), h, i, k, j, l, e = a > 0.3 ? [1, 2, 4] : a > 0.15 ? [1, 2, 4, 6, 8] : [1, 2, 3, 4, 5, 6, 7, 8, 9]; f < c + 1 && !l; f++) {
            i = e.length;
            for (h = 0; h < i && !l; h++) k = Ga(oa(f) * e[h]),
            k > b && (!d || j <= c) && g.push(j),
            j > c && (l = !0),
            j = k
        } else if (b = oa(b), c = oa(c), a = e[d ? "minorTickInterval": "tickInterval"], a = o(a === "auto" ? null: a, this._minorAutoInterval, (c - b) * (e.tickPixelInterval / (d ? 5 : 1)) / ((d ? f / this.tickPositions.length: f) || 1)), a = sb(a, null, rb(a)), g = xa(this.getLinearTickPositions(a, b, c), Ga), !d) this._minorAutoInterval = a / 5;
        if (!d) this.tickInterval = a;
        return g
    };
    var Hb = P.Tooltip = function() {
        this.init.apply(this, arguments)
    };
    Hb.prototype = {
        init: function(a, b) {
            var c = b.borderWidth,
            d = b.style,
            e = I(d.padding);
            this.chart = a;
            this.options = b;
            this.crosshairs = [];
            this.now = {
                x: 0,
                y: 0
            };
            this.isHidden = !0;
            this.label = a.renderer.label("", 0, 0, b.shape || "callout", null, null, b.useHTML, null, "tooltip").attr({
                padding: e,
                fill: b.backgroundColor,
                "stroke-width": c,
                r: b.borderRadius,
                zIndex: 8
            }).css(d).css({
                padding: 0
            }).add().attr({
                y: -9999
            });
            ja || this.label.shadow(b.shadow);
            this.shared = b.shared
        },
        destroy: function() {
            if (this.label) this.label = this.label.destroy();
            clearTimeout(this.hideTimer);
            clearTimeout(this.tooltipTimeout)
        },
        move: function(a, b, c, d) {
            var e = this,
            f = e.now,
            g = e.options.animation !== !1 && !e.isHidden,
            h = e.followPointer || e.len > 1;
            v(f, {
                x: g ? (2 * f.x + a) / 3 : a,
                y: g ? (f.y + b) / 2 : b,
                anchorX: h ? r: g ? (2 * f.anchorX + c) / 3 : c,
                anchorY: h ? r: g ? (f.anchorY + d) / 2 : d
            });
            e.label.attr(f);
            if (g && (M(a - f.x) > 1 || M(b - f.y) > 1)) clearTimeout(this.tooltipTimeout),
            this.tooltipTimeout = setTimeout(function() {
                e && e.move(a, b, c, d)
            },
            32)
        },
        hide: function() {
            var a = this,
            b;
            clearTimeout(this.hideTimer);
            if (!this.isHidden) b = this.chart.hoverPoints,
            this.hideTimer = setTimeout(function() {
                a.label.fadeOut();
                a.isHidden = !0
            },
            o(this.options.hideDelay, 500)),
            b && q(b,
            function(a) {
                a.setState()
            }),
            this.chart.hoverPoints = null
        },
        getAnchor: function(a, b) {
            var c, d = this.chart,
            e = d.inverted,
            f = d.plotTop,
            g = 0,
            h = 0,
            i, a = ma(a);
            c = a[0].tooltipPos;
            this.followPointer && b && (b.chartX === r && (b = d.pointer.normalize(b)), c = [b.chartX - d.plotLeft, b.chartY - f]);
            c || (q(a,
            function(a) {
                i = a.series.yAxis;
                g += a.plotX;
                h += (a.plotLow ? (a.plotLow + a.plotHigh) / 2 : a.plotY) + (!e && i ? i.top - f: 0)
            }), g /= a.length, h /= a.length, c = [e ? d.plotWidth - h: g, this.shared && !e && a.length > 1 && b ? b.chartY - f: e ? d.plotHeight - g: h]);
            return xa(c, s)
        },
        getPosition: function(a, b, c) {
            var d = this.chart,
            e = this.distance,
            f = {},
            g, h = ["y", d.chartHeight, b, c.plotY + d.plotTop],
            i = ["x", d.chartWidth, a, c.plotX + d.plotLeft],
            k = c.ttBelow || d.inverted && !c.negative || !d.inverted && c.negative,
            j = function(a, b, c, d) {
                var g = c < d - e,
                b = d + e + c < b,
                c = d - e - c;
                d += e;
                if (k && b) f[a] = d;
                else if (!k && g) f[a] = c;
                else if (g) f[a] = c;
                else if (b) f[a] = d;
                else return ! 1
            },
            l = function(a, b, c, d) {
                if (d < e || d > b - e) return ! 1;
                else f[a] = d < c / 2 ? 1 : d > b - c / 2 ? b - c - 2 : d - c / 2
            },
            m = function(a) {
                var b = h;
                h = i;
                i = b;
                g = a
            },
            n = function() {
                j.apply(0, h) !== !1 ? l.apply(0, i) === !1 && !g && (m(!0), n()) : g ? f.x = f.y = 0 : (m(!0), n())
            }; (d.inverted || this.len > 1) && m();
            n();
            return f
        },
        defaultFormatter: function(a) {
            var b = this.points || ma(this),
            c = b[0].series,
            d;
            d = [a.tooltipHeaderFormatter(b[0])];
            q(b,
            function(a) {
                c = a.series;
                d.push(c.tooltipFormatter && c.tooltipFormatter(a) || a.point.tooltipFormatter(c.tooltipOptions.pointFormat))
            });
            d.push(a.options.footerFormat || "");
            return d.join("")
        },
        refresh: function(a, b) {
            var c = this.chart,
            d = this.label,
            e = this.options,
            f, g, h = {},
            i, k = [];
            i = e.formatter || this.defaultFormatter;
            var h = c.hoverPoints,
            j, l = this.shared;
            clearTimeout(this.hideTimer);
            this.followPointer = ma(a)[0].series.tooltipOptions.followPointer;
            g = this.getAnchor(a, b);
            f = g[0];
            g = g[1];
            l && (!a.series || !a.series.noSharedTooltip) ? (c.hoverPoints = a, h && q(h,
            function(a) {
                a.setState()
            }), q(a,
            function(a) {
                a.setState("hover");
                k.push(a.getLabelConfig())
            }), h = {
                x: a[0].category,
                y: a[0].y
            },
            h.points = k, this.len = k.length, a = a[0]) : h = a.getLabelConfig();
            i = i.call(h, this);
            h = a.series;
            this.distance = o(h.tooltipOptions.distance, 16);
            i === !1 ? this.hide() : (this.isHidden && (eb(d), d.attr("opacity", 1).show()), d.attr({
                text: i
            }), j = e.borderColor || a.color || h.color || "#606060", d.attr({
                stroke: j
            }), this.updatePosition({
                plotX: f,
                plotY: g,
                negative: a.negative,
                ttBelow: a.ttBelow
            }), this.isHidden = !1);
            N(c, "tooltipRefresh", {
                text: i,
                x: f + c.plotLeft,
                y: g + c.plotTop,
                borderColor: j
            })
        },
        updatePosition: function(a) {
            var b = this.chart,
            c = this.label,
            c = (this.options.positioner || this.getPosition).call(this, c.width, c.height, a);
            this.move(s(c.x), s(c.y), a.plotX + b.plotLeft, a.plotY + b.plotTop)
        },
        tooltipHeaderFormatter: function(a) {
            var b = a.series,
            c = b.tooltipOptions,
            d = c.dateTimeLabelFormats,
            e = c.xDateFormat,
            f = b.xAxis,
            g = f && f.options.type === "datetime" && la(a.key),
            c = c.headerFormat,
            f = f && f.closestPointRange,
            h;
            if (g && !e) {
                if (f) for (h in H) {
                    if (H[h] >= f || H[h] <= H.day && a.key % H[h] > 0) {
                        e = d[h];
                        break
                    }
                } else e = d.day;
                e = e || d.year
            }
            g && e && (c = c.replace("{point.key}", "{point.key:" + e + "}"));
            return Ja(c, {
                point: a,
                series: b
            })
        }
    };
    var sa;
    ab = B.documentElement.ontouchstart !== r;
    var Xa = P.Pointer = function(a, b) {
        this.init(a, b)
    };
    Xa.prototype = {
        init: function(a, b) {
            var c = b.chart,
            d = c.events,
            e = ja ? "": c.zoomType,
            c = a.inverted,
            f;
            this.options = b;
            this.chart = a;
            this.zoomX = f = /x/.test(e);
            this.zoomY = e = /y/.test(e);
            this.zoomHor = f && !c || e && c;
            this.zoomVert = e && !c || f && c;
            this.hasZoom = f || e;
            this.runChartClick = d && !!d.click;
            this.pinchDown = [];
            this.lastValidTouch = {};
            if (P.Tooltip && b.tooltip.enabled) a.tooltip = new Hb(a, b.tooltip),
            this.followTouchMove = b.tooltip.followTouchMove;
            this.setDOMEvents()
        },
        normalize: function(a, b) {
            var c, d, a = a || window.event,
            a = cc(a);
            if (!a.target) a.target = a.srcElement;
            d = a.touches ? a.touches.length ? a.touches.item(0) : a.changedTouches[0] : a;
            if (!b) this.chartPosition = b = bc(this.chart.container);
            d.pageX === r ? (c = w(a.x, a.clientX - b.left), d = a.y) : (c = d.pageX - b.left, d = d.pageY - b.top);
            return v(a, {
                chartX: s(c),
                chartY: s(d)
            })
        },
        getCoordinates: function(a) {
            var b = {
                xAxis: [],
                yAxis: []
            };
            q(this.chart.axes,
            function(c) {
                b[c.isXAxis ? "xAxis": "yAxis"].push({
                    axis: c,
                    value: c.toValue(a[c.horiz ? "chartX": "chartY"])
                })
            });
            return b
        },
        getIndex: function(a) {
            var b = this.chart;
            return b.inverted ? b.plotHeight + b.plotTop - a.chartY: a.chartX - b.plotLeft
        },
        runPointActions: function(a) {
            var b = this.chart,
            c = b.series,
            d = b.tooltip,
            e, f, g = b.hoverPoint,
            h = b.hoverSeries,
            i, k, j = b.chartWidth,
            l = this.getIndex(a);
            if (d && this.options.tooltip.shared && (!h || !h.noSharedTooltip)) {
                f = [];
                i = c.length;
                for (k = 0; k < i; k++) if (c[k].visible && c[k].options.enableMouseTracking !== !1 && !c[k].noSharedTooltip && c[k].singularTooltips !== !0 && c[k].tooltipPoints.length && (e = c[k].tooltipPoints[l]) && e.series) e._dist = M(l - e.clientX),
                j = z(j, e._dist),
                f.push(e);
                for (i = f.length; i--;) f[i]._dist > j && f.splice(i, 1);
                if (f.length && f[0].clientX !== this.hoverX) d.refresh(f, a),
                this.hoverX = f[0].clientX
            }
            c = h && h.tooltipOptions.followPointer;
            if (h && h.tracker && !c) {
                if ((e = h.tooltipPoints[l]) && e !== g) e.onMouseOver(a)
            } else d && c && !d.isHidden && (h = d.getAnchor([{}], a), d.updatePosition({
                plotX: h[0],
                plotY: h[1]
            }));
            if (d && !this._onDocumentMouseMove) this._onDocumentMouseMove = function(a) {
                if (aa[sa]) aa[sa].pointer.onDocumentMouseMove(a)
            },
            A(B, "mousemove", this._onDocumentMouseMove);
            q(b.axes,
            function(b) {
                b.drawCrosshair(a, o(e, g))
            })
        },
        reset: function(a) {
            var b = this.chart,
            c = b.hoverSeries,
            d = b.hoverPoint,
            e = b.tooltip,
            f = e && e.shared ? b.hoverPoints: d; (a = a && e && f) && ma(f)[0].plotX === r && (a = !1);
            if (a) e.refresh(f),
            d && d.setState(d.state, !0);
            else {
                if (d) d.onMouseOut();
                if (c) c.onMouseOut();
                e && e.hide();
                if (this._onDocumentMouseMove) R(B, "mousemove", this._onDocumentMouseMove),
                this._onDocumentMouseMove = null;
                q(b.axes,
                function(a) {
                    a.hideCrosshair()
                });
                this.hoverX = null
            }
        },
        scaleGroups: function(a, b) {
            var c = this.chart,
            d;
            q(c.series,
            function(e) {
                d = a || e.getPlotBox();
                e.xAxis && e.xAxis.zoomEnabled && (e.group.attr(d), e.markerGroup && (e.markerGroup.attr(d), e.markerGroup.clip(b ? c.clipRect: null)), e.dataLabelsGroup && e.dataLabelsGroup.attr(d))
            });
            c.clipRect.attr(b || c.clipBox)
        },
        dragStart: function(a) {
            var b = this.chart;
            b.mouseIsDown = a.type;
            b.cancelClick = !1;
            b.mouseDownX = this.mouseDownX = a.chartX;
            b.mouseDownY = this.mouseDownY = a.chartY
        },
        drag: function(a) {
            var b = this.chart,
            c = b.options.chart,
            d = a.chartX,
            e = a.chartY,
            f = this.zoomHor,
            g = this.zoomVert,
            h = b.plotLeft,
            i = b.plotTop,
            k = b.plotWidth,
            j = b.plotHeight,
            l, m = this.mouseDownX,
            n = this.mouseDownY;
            d < h ? d = h: d > h + k && (d = h + k);
            e < i ? e = i: e > i + j && (e = i + j);
            this.hasDragged = Math.sqrt(Math.pow(m - d, 2) + Math.pow(n - e, 2));
            if (this.hasDragged > 10) {
                l = b.isInsidePlot(m - h, n - i);
                if (b.hasCartesianSeries && (this.zoomX || this.zoomY) && l && !this.selectionMarker) this.selectionMarker = b.renderer.rect(h, i, f ? 1 : k, g ? 1 : j, 0).attr({
                    fill: c.selectionMarkerFill || "rgba(69,114,167,0.25)",
                    zIndex: 7
                }).add();
                this.selectionMarker && f && (d -= m, this.selectionMarker.attr({
                    width: M(d),
                    x: (d > 0 ? 0 : d) + m
                }));
                this.selectionMarker && g && (d = e - n, this.selectionMarker.attr({
                    height: M(d),
                    y: (d > 0 ? 0 : d) + n
                }));
                l && !this.selectionMarker && c.panning && b.pan(a, c.panning)
            }
        },
        drop: function(a) {
            var b = this.chart,
            c = this.hasPinched;
            if (this.selectionMarker) {
                var d = {
                    xAxis: [],
                    yAxis: [],
                    originalEvent: a.originalEvent || a
                },
                a = this.selectionMarker,
                e = a.attr ? a.attr("x") : a.x,
                f = a.attr ? a.attr("y") : a.y,
                g = a.attr ? a.attr("width") : a.width,
                h = a.attr ? a.attr("height") : a.height,
                i;
                if (this.hasDragged || c) q(b.axes,
                function(a) {
                    if (a.zoomEnabled) {
                        var b = a.horiz,
                        c = a.toValue(b ? e: f),
                        b = a.toValue(b ? e + g: f + h); ! isNaN(c) && !isNaN(b) && (d[a.coll].push({
                            axis: a,
                            min: z(c, b),
                            max: w(c, b)
                        }), i = !0)
                    }
                }),
                i && N(b, "selection", d,
                function(a) {
                    b.zoom(v(a, c ? {
                        animation: !1
                    }: null))
                });
                this.selectionMarker = this.selectionMarker.destroy();
                c && this.scaleGroups()
            }
            if (b) E(b.container, {
                cursor: b._cursor
            }),
            b.cancelClick = this.hasDragged > 10,
            b.mouseIsDown = this.hasDragged = this.hasPinched = !1,
            this.pinchDown = []
        },
        onContainerMouseDown: function(a) {
            a = this.normalize(a);
            a.preventDefault && a.preventDefault();
            this.dragStart(a)
        },
        onDocumentMouseUp: function(a) {
            aa[sa] && aa[sa].pointer.drop(a)
        },
        onDocumentMouseMove: function(a) {
            var b = this.chart,
            c = this.chartPosition,
            d = b.hoverSeries,
            a = this.normalize(a, c);
            c && d && !this.inClass(a.target, "highcharts-tracker") && !b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop) && this.reset()
        },
        onContainerMouseLeave: function() {
            var a = aa[sa];
            if (a) a.pointer.reset(),
            a.pointer.chartPosition = null
        },
        onContainerMouseMove: function(a) {
            var b = this.chart;
            sa = b.index;
            a = this.normalize(a);
            b.mouseIsDown === "mousedown" && this.drag(a); (this.inClass(a.target, "highcharts-tracker") || b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop)) && !b.openMenu && this.runPointActions(a)
        },
        inClass: function(a, b) {
            for (var c; a;) {
                if (c = W(a, "class")) if (c.indexOf(b) !== -1) return ! 0;
                else if (c.indexOf("highcharts-container") !== -1) return ! 1;
                a = a.parentNode
            }
        },
        onTrackerMouseOut: function(a) {
            var b = this.chart.hoverSeries,
            c = (a = a.relatedTarget || a.toElement) && a.point && a.point.series;
            if (b && !b.options.stickyTracking && !this.inClass(a, "highcharts-tooltip") && c !== b) b.onMouseOut()
        },
        onContainerClick: function(a) {
            var b = this.chart,
            c = b.hoverPoint,
            d = b.plotLeft,
            e = b.plotTop,
            a = this.normalize(a);
            a.cancelBubble = !0;
            b.cancelClick || (c && this.inClass(a.target, "highcharts-tracker") ? (N(c.series, "click", v(a, {
                point: c
            })), b.hoverPoint && c.firePointEvent("click", a)) : (v(a, this.getCoordinates(a)), b.isInsidePlot(a.chartX - d, a.chartY - e) && N(b, "click", a)))
        },
        setDOMEvents: function() {
            var a = this,
            b = a.chart.container;
            b.onmousedown = function(b) {
                a.onContainerMouseDown(b)
            };
            b.onmousemove = function(b) {
                a.onContainerMouseMove(b)
            };
            b.onclick = function(b) {
                a.onContainerClick(b)
            };
            A(b, "mouseleave", a.onContainerMouseLeave);
            db === 1 && A(B, "mouseup", a.onDocumentMouseUp);
            if (ab) b.ontouchstart = function(b) {
                a.onContainerTouchStart(b)
            },
            b.ontouchmove = function(b) {
                a.onContainerTouchMove(b)
            },
            db === 1 && A(B, "touchend", a.onDocumentTouchEnd)
        },
        destroy: function() {
            var a;
            R(this.chart.container, "mouseleave", this.onContainerMouseLeave);
            db || (R(B, "mouseup", this.onDocumentMouseUp), R(B, "touchend", this.onDocumentTouchEnd));
            clearInterval(this.tooltipTimeout);
            for (a in this) this[a] = null
        }
    };
    v(P.Pointer.prototype, {
        pinchTranslate: function(a, b, c, d, e, f) { (this.zoomHor || this.pinchHor) && this.pinchTranslateDirection(!0, a, b, c, d, e, f); (this.zoomVert || this.pinchVert) && this.pinchTranslateDirection(!1, a, b, c, d, e, f)
        },
        pinchTranslateDirection: function(a, b, c, d, e, f, g, h) {
            var i = this.chart,
            k = a ? "x": "y",
            j = a ? "X": "Y",
            l = "chart" + j,
            m = a ? "width": "height",
            n = i["plot" + (a ? "Left": "Top")],
            p,
            u,
            o = h || 1,
            q = i.inverted,
            x = i.bounds[a ? "h": "v"],
            r = b.length === 1,
            s = b[0][l],
            t = c[0][l],
            w = !r && b[1][l],
            v = !r && c[1][l],
            y,
            c = function() { ! r && M(s - w) > 20 && (o = h || M(t - v) / M(s - w));
                u = (n - t) / o + s;
                p = i["plot" + (a ? "Width": "Height")] / o
            };
            c();
            b = u;
            b < x.min ? (b = x.min, y = !0) : b + p > x.max && (b = x.max - p, y = !0);
            y ? (t -= 0.8 * (t - g[k][0]), r || (v -= 0.8 * (v - g[k][1])), c()) : g[k] = [t, v];
            q || (f[k] = u - n, f[m] = p);
            f = q ? 1 / o: o;
            e[m] = p;
            e[k] = b;
            d[q ? a ? "scaleY": "scaleX": "scale" + j] = o;
            d["translate" + j] = f * n + (t - f * s)
        },
        pinch: function(a) {
            var b = this,
            c = b.chart,
            d = b.pinchDown,
            e = b.followTouchMove,
            f = a.touches,
            g = f.length,
            h = b.lastValidTouch,
            i = b.hasZoom,
            k = b.selectionMarker,
            j = {},
            l = g === 1 && (b.inClass(a.target, "highcharts-tracker") && c.runTrackerClick || c.runChartClick),
            m = {}; (i || e) && !l && a.preventDefault();
            xa(f,
            function(a) {
                return b.normalize(a)
            });
            if (a.type === "touchstart") q(f,
            function(a, b) {
                d[b] = {
                    chartX: a.chartX,
                    chartY: a.chartY
                }
            }),
            h.x = [d[0].chartX, d[1] && d[1].chartX],
            h.y = [d[0].chartY, d[1] && d[1].chartY],
            q(c.axes,
            function(a) {
                if (a.zoomEnabled) {
                    var b = c.bounds[a.horiz ? "h": "v"],
                    d = a.minPixelPadding,
                    e = a.toPixels(a.dataMin),
                    f = a.toPixels(a.dataMax),
                    g = z(e, f),
                    e = w(e, f);
                    b.min = z(a.pos, g - d);
                    b.max = w(a.pos + a.len, e + d)
                }
            });
            else if (d.length) {
                if (!k) b.selectionMarker = k = v({
                    destroy: na
                },
                c.plotBox);
                b.pinchTranslate(d, f, j, k, m, h);
                b.hasPinched = i;
                b.scaleGroups(j, m); ! i && e && g === 1 && this.runPointActions(b.normalize(a))
            }
        },
        onContainerTouchStart: function(a) {
            var b = this.chart;
            sa = b.index;
            a.touches.length === 1 ? (a = this.normalize(a), b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop) ? (this.runPointActions(a), this.pinch(a)) : this.reset()) : a.touches.length === 2 && this.pinch(a)
        },
        onContainerTouchMove: function(a) { (a.touches.length === 1 || a.touches.length === 2) && this.pinch(a)
        },
        onDocumentTouchEnd: function(a) {
            aa[sa] && aa[sa].pointer.drop(a)
        }
    });
    if (X.PointerEvent || X.MSPointerEvent) {
        var ya = {},
        Ib = !!X.PointerEvent,
        gc = function() {
            var a, b = [];
            b.item = function(a) {
                return this[a]
            };
            for (a in ya) ya.hasOwnProperty(a) && b.push({
                pageX: ya[a].pageX,
                pageY: ya[a].pageY,
                target: ya[a].target
            });
            return b
        },
        Jb = function(a, b, c, d) {
            a = a.originalEvent || a;
            if ((a.pointerType === "touch" || a.pointerType === a.MSPOINTER_TYPE_TOUCH) && aa[sa]) d(a),
            d = aa[sa].pointer,
            d[b]({
                type: c,
                target: a.currentTarget,
                preventDefault: na,
                touches: gc()
            })
        };
        v(Xa.prototype, {
            onContainerPointerDown: function(a) {
                Jb(a, "onContainerTouchStart", "touchstart",
                function(a) {
                    ya[a.pointerId] = {
                        pageX: a.pageX,
                        pageY: a.pageY,
                        target: a.currentTarget
                    }
                })
            },
            onContainerPointerMove: function(a) {
                Jb(a, "onContainerTouchMove", "touchmove",
                function(a) {
                    ya[a.pointerId] = {
                        pageX: a.pageX,
                        pageY: a.pageY
                    };
                    if (!ya[a.pointerId].target) ya[a.pointerId].target = a.currentTarget
                })
            },
            onDocumentPointerUp: function(a) {
                Jb(a, "onContainerTouchEnd", "touchend",
                function(a) {
                    delete ya[a.pointerId]
                })
            },
            batchMSEvents: function(a) {
                a(this.chart.container, Ib ? "pointerdown": "MSPointerDown", this.onContainerPointerDown);
                a(this.chart.container, Ib ? "pointermove": "MSPointerMove", this.onContainerPointerMove);
                a(B, Ib ? "pointerup": "MSPointerUp", this.onDocumentPointerUp)
            }
        });
        O(Xa.prototype, "init",
        function(a, b, c) {
            a.call(this, b, c); (this.hasZoom || this.followTouchMove) && E(b.container, {
                "-ms-touch-action": Y,
                "touch-action": Y
            })
        });
        O(Xa.prototype, "setDOMEvents",
        function(a) {
            a.apply(this); (this.hasZoom || this.followTouchMove) && this.batchMSEvents(A)
        });
        O(Xa.prototype, "destroy",
        function(a) {
            this.batchMSEvents(R);
            a.call(this)
        })
    }
    var pb = P.Legend = function(a, b) {
        this.init(a, b)
    };
    pb.prototype = {
        init: function(a, b) {
            var c = this,
            d = b.itemStyle,
            e = o(b.padding, 8),
            f = b.itemMarginTop || 0;
            this.options = b;
            if (b.enabled) c.baseline = I(d.fontSize) + 3 + f,
            c.itemStyle = d,
            c.itemHiddenStyle = y(d, b.itemHiddenStyle),
            c.itemMarginTop = f,
            c.padding = e,
            c.initialItemX = e,
            c.initialItemY = e - 5,
            c.maxItemWidth = 0,
            c.chart = a,
            c.itemHeight = 0,
            c.lastLineHeight = 0,
            c.symbolWidth = o(b.symbolWidth, 16),
            c.pages = [],
            c.render(),
            A(c.chart, "endResize",
            function() {
                c.positionCheckboxes()
            })
        },
        colorizeItem: function(a, b) {
            var c = this.options,
            d = a.legendItem,
            e = a.legendLine,
            f = a.legendSymbol,
            g = this.itemHiddenStyle.color,
            c = b ? c.itemStyle.color: g,
            h = b ? a.legendColor || a.color || "#CCC": g,
            g = a.options && a.options.marker,
            i = {
                fill: h
            },
            k;
            d && d.css({
                fill: c,
                color: c
            });
            e && e.attr({
                stroke: h
            });
            if (f) {
                if (g && f.isMarker) for (k in i.stroke = h, g = a.convertAttribs(g), g) d = g[k],
                d !== r && (i[k] = d);
                f.attr(i)
            }
        },
        positionItem: function(a) {
            var b = this.options,
            c = b.symbolPadding,
            b = !b.rtl,
            d = a._legendItemPos,
            e = d[0],
            d = d[1],
            f = a.checkbox;
            a.legendGroup && a.legendGroup.translate(b ? e: this.legendWidth - e - 2 * c - 4, d);
            if (f) f.x = e,
            f.y = d
        },
        destroyItem: function(a) {
            var b = a.checkbox;
            q(["legendItem", "legendLine", "legendSymbol", "legendGroup"],
            function(b) {
                a[b] && (a[b] = a[b].destroy())
            });
            b && Sa(a.checkbox)
        },
        destroy: function() {
            var a = this.group,
            b = this.box;
            if (b) this.box = b.destroy();
            if (a) this.group = a.destroy()
        },
        positionCheckboxes: function(a) {
            var b = this.group.alignAttr,
            c, d = this.clipHeight || this.legendHeight;
            if (b) c = b.translateY,
            q(this.allItems,
            function(e) {
                var f = e.checkbox,
                g;
                f && (g = c + f.y + (a || 0) + 3, E(f, {
                    left: b.translateX + e.checkboxOffset + f.x - 20 + "px",
                    top: g + "px",
                    display: g > c - 6 && g < c + d - 6 ? "": Y
                }))
            })
        },
        renderTitle: function() {
            var a = this.padding,
            b = this.options.title,
            c = 0;
            if (b.text) {
                if (!this.title) this.title = this.chart.renderer.label(b.text, a - 3, a - 4, null, null, null, null, null, "legend-title").attr({
                    zIndex: 1
                }).css(b.style).add(this.group);
                a = this.title.getBBox();
                c = a.height;
                this.offsetWidth = a.width;
                this.contentGroup.attr({
                    translateY: c
                })
            }
            this.titleHeight = c
        },
        renderItem: function(a) {
            var b = this.chart,
            c = b.renderer,
            d = this.options,
            e = d.layout === "horizontal",
            f = this.symbolWidth,
            g = d.symbolPadding,
            h = this.itemStyle,
            i = this.itemHiddenStyle,
            k = this.padding,
            j = e ? o(d.itemDistance, 20) : 0,
            l = !d.rtl,
            m = d.width,
            n = d.itemMarginBottom || 0,
            p = this.itemMarginTop,
            u = this.initialItemX,
            q = a.legendItem,
            G = a.series && a.series.drawLegendSymbol ? a.series: a,
            x = G.options,
            x = this.createCheckboxForItem && x && x.showCheckbox,
            r = d.useHTML;
            if (!q) a.legendGroup = c.g("legend-item").attr({
                zIndex: 1
            }).add(this.scrollGroup),
            G.drawLegendSymbol(this, a),
            a.legendItem = q = c.text(d.labelFormat ? Ja(d.labelFormat, a) : d.labelFormatter.call(a), l ? f + g: -g, this.baseline, r).css(y(a.visible ? h: i)).attr({
                align: l ? "left": "right",
                zIndex: 2
            }).add(a.legendGroup),
            this.setItemEvents && this.setItemEvents(a, q, r, h, i),
            this.colorizeItem(a, a.visible),
            x && this.createCheckboxForItem(a);
            c = q.getBBox();
            f = a.checkboxOffset = d.itemWidth || a.legendItemWidth || f + g + c.width + j + (x ? 20 : 0);
            this.itemHeight = g = s(a.legendItemHeight || c.height);
            if (e && this.itemX - u + f > (m || b.chartWidth - 2 * k - u - d.x)) this.itemX = u,
            this.itemY += p + this.lastLineHeight + n,
            this.lastLineHeight = 0;
            this.maxItemWidth = w(this.maxItemWidth, f);
            this.lastItemY = p + this.itemY + n;
            this.lastLineHeight = w(g, this.lastLineHeight);
            a._legendItemPos = [this.itemX, this.itemY];
            e ? this.itemX += f: (this.itemY += p + g + n, this.lastLineHeight = g);
            this.offsetWidth = m || w((e ? this.itemX - u - j: f) + k, this.offsetWidth)
        },
        getAllItems: function() {
            var a = [];
            q(this.chart.series,
            function(b) {
                var c = b.options;
                if (o(c.showInLegend, !t(c.linkedTo) ? r: !1, !0)) a = a.concat(b.legendItems || (c.legendType === "point" ? b.data: b))
            });
            return a
        },
        render: function() {
            var a = this,
            b = a.chart,
            c = b.renderer,
            d = a.group,
            e, f, g, h, i = a.box,
            k = a.options,
            j = a.padding,
            l = k.borderWidth,
            m = k.backgroundColor;
            a.itemX = a.initialItemX;
            a.itemY = a.initialItemY;
            a.offsetWidth = 0;
            a.lastItemY = 0;
            if (!d) a.group = d = c.g("legend").attr({
                zIndex: 7
            }).add(),
            a.contentGroup = c.g().attr({
                zIndex: 1
            }).add(d),
            a.scrollGroup = c.g().add(a.contentGroup);
            a.renderTitle();
            e = a.getAllItems();
            tb(e,
            function(a, b) {
                return (a.options && a.options.legendIndex || 0) - (b.options && b.options.legendIndex || 0)
            });
            k.reversed && e.reverse();
            a.allItems = e;
            a.display = f = !!e.length;
            q(e,
            function(b) {
                a.renderItem(b)
            });
            g = k.width || a.offsetWidth;
            h = a.lastItemY + a.lastLineHeight + a.titleHeight;
            h = a.handleOverflow(h);
            if (l || m) {
                g += j;
                h += j;
                if (i) {
                    if (g > 0 && h > 0) i[i.isNew ? "attr": "animate"](i.crisp({
                        width: g,
                        height: h
                    })),
                    i.isNew = !1
                } else a.box = i = c.rect(0, 0, g, h, k.borderRadius, l || 0).attr({
                    stroke: k.borderColor,
                    "stroke-width": l || 0,
                    fill: m || Y
                }).add(d).shadow(k.shadow),
                i.isNew = !0;
                i[f ? "show": "hide"]()
            }
            a.legendWidth = g;
            a.legendHeight = h;
            q(e,
            function(b) {
                a.positionItem(b)
            });
            f && d.align(v({
                width: g,
                height: h
            },
            k), !0, "spacingBox");
            b.isResizing || this.positionCheckboxes()
        },
        handleOverflow: function(a) {
            var b = this,
            c = this.chart,
            d = c.renderer,
            e = this.options,
            f = e.y,
            f = c.spacingBox.height + (e.verticalAlign === "top" ? -f: f) - this.padding,
            g = e.maxHeight,
            h,
            i = this.clipRect,
            k = e.navigation,
            j = o(k.animation, !0),
            l = k.arrowSize || 12,
            m = this.nav,
            n = this.pages,
            p,
            u = this.allItems;
            e.layout === "horizontal" && (f /= 2);
            g && (f = z(f, g));
            n.length = 0;
            if (a > f && !e.useHTML) {
                this.clipHeight = h = f - 20 - this.titleHeight - this.padding;
                this.currentPage = o(this.currentPage, 1);
                this.fullHeight = a;
                q(u,
                function(a, b) {
                    var c = a._legendItemPos[1],
                    d = s(a.legendItem.getBBox().height),
                    e = n.length;
                    if (!e || c - n[e - 1] > h && (p || c) !== n[e - 1]) n.push(p || c),
                    e++;
                    b === u.length - 1 && c + d - n[e - 1] > h && n.push(c);
                    c !== p && (p = c)
                });
                if (!i) i = b.clipRect = d.clipRect(0, this.padding, 9999, 0),
                b.contentGroup.clip(i);
                i.attr({
                    height: h
                });
                if (!m) this.nav = m = d.g().attr({
                    zIndex: 1
                }).add(this.group),
                this.up = d.symbol("triangle", 0, 0, l, l).on("click",
                function() {
                    b.scroll( - 1, j)
                }).add(m),
                this.pager = d.text("", 15, 10).css(k.style).add(m),
                this.down = d.symbol("triangle-down", 0, 0, l, l).on("click",
                function() {
                    b.scroll(1, j)
                }).add(m);
                b.scroll(0);
                a = f
            } else if (m) i.attr({
                height: c.chartHeight
            }),
            m.hide(),
            this.scrollGroup.attr({
                translateY: 1
            }),
            this.clipHeight = 0;
            return a
        },
        scroll: function(a, b) {
            var c = this.pages,
            d = c.length,
            e = this.currentPage + a,
            f = this.clipHeight,
            g = this.options.navigation,
            h = g.activeColor,
            g = g.inactiveColor,
            i = this.pager,
            k = this.padding;
            e > d && (e = d);
            if (e > 0) b !== r && Ya(b, this.chart),
            this.nav.attr({
                translateX: k,
                translateY: f + this.padding + 7 + this.titleHeight,
                visibility: "visible"
            }),
            this.up.attr({
                fill: e === 1 ? g: h
            }).css({
                cursor: e === 1 ? "default": "pointer"
            }),
            i.attr({
                text: e + "/" + d
            }),
            this.down.attr({
                x: 18 + this.pager.getBBox().width,
                fill: e === d ? g: h
            }).css({
                cursor: e === d ? "default": "pointer"
            }),
            c = -c[e - 1] + this.initialItemY,
            this.scrollGroup.animate({
                translateY: c
            }),
            this.currentPage = e,
            this.positionCheckboxes(c)
        }
    };
    Q = P.LegendSymbolMixin = {
        drawRectangle: function(a, b) {
            var c = a.options.symbolHeight || 12;
            b.legendSymbol = this.chart.renderer.rect(0, a.baseline - 5 - c / 2, a.symbolWidth, c, a.options.symbolRadius || 0).attr({
                zIndex: 3
            }).add(b.legendGroup)
        },
        drawLineMarker: function(a) {
            var b = this.options,
            c = b.marker,
            d;
            d = a.symbolWidth;
            var e = this.chart.renderer,
            f = this.legendGroup,
            a = a.baseline - s(e.fontMetrics(a.options.itemStyle.fontSize).b * 0.3),
            g;
            if (b.lineWidth) {
                g = {
                    "stroke-width": b.lineWidth
                };
                if (b.dashStyle) g.dashstyle = b.dashStyle;
                this.legendLine = e.path(["M", 0, a, "L", d, a]).attr(g).add(f)
            }
            if (c && c.enabled !== !1) b = c.radius,
            this.legendSymbol = d = e.symbol(this.symbol, d / 2 - b, a - b, 2 * b, 2 * b).add(f),
            d.isMarker = !0
        }
    }; (/Trident\/7\.0/.test(Da) || $a) && O(pb.prototype, "positionItem",
    function(a, b) {
        var c = this,
        d = function() {
            b._legendItemPos && a.call(c, b)
        };
        d();
        setTimeout(d)
    });
    va.prototype = {
        init: function(a, b) {
            var c, d = a.series;
            a.series = null;
            c = y(F, a);
            c.series = a.series = d;
            this.userOptions = a;
            d = c.chart;
            this.margin = this.splashArray("margin", d);
            this.spacing = this.splashArray("spacing", d);
            var e = d.events;
            this.bounds = {
                h: {},
                v: {}
            };
            this.callback = b;
            this.isResizing = 0;
            this.options = c;
            this.axes = [];
            this.series = [];
            this.hasCartesianSeries = d.showAxes;
            var f = this,
            g;
            f.index = aa.length;
            aa.push(f);
            db++;
            d.reflow !== !1 && A(f, "load",
            function() {
                f.initReflow()
            });
            if (e) for (g in e) A(f, g, e[g]);
            f.xAxis = [];
            f.yAxis = [];
            f.animation = ja ? !1 : o(d.animation, !0);
            f.pointCount = 0;
            f.counters = new Lb;
            f.firstRender()
        },
        initSeries: function(a) {
            var b = this.options.chart; (b = C[a.type || b.type || b.defaultSeriesType]) || qa(17, !0);
            b = new b;
            b.init(this, a);
            return b
        },
        isInsidePlot: function(a, b, c) {
            var d = c ? b: a,
            a = c ? a: b;
            return d >= 0 && d <= this.plotWidth && a >= 0 && a <= this.plotHeight
        },
        adjustTickAmounts: function() {
            this.options.chart.alignTicks !== !1 && q(this.axes,
            function(a) {
                a.adjustTickAmount()
            });
            this.maxTicks = null
        },
        redraw: function(a) {
            var b = this.axes,
            c = this.series,
            d = this.pointer,
            e = this.legend,
            f = this.isDirtyLegend,
            g, h, i = this.isDirtyBox,
            k = c.length,
            j = k,
            l = this.renderer,
            m = l.isHidden(),
            n = [];
            Ya(a, this);
            m && this.cloneRenderTo();
            for (this.layOutTitles(); j--;) if (a = c[j], a.options.stacking && (g = !0, a.isDirty)) {
                h = !0;
                break
            }
            if (h) for (j = k; j--;) if (a = c[j], a.options.stacking) a.isDirty = !0;
            q(c,
            function(a) {
                a.isDirty && a.options.legendType === "point" && (f = !0)
            });
            if (f && e.options.enabled) e.render(),
            this.isDirtyLegend = !1;
            g && this.getStacks();
            if (this.hasCartesianSeries) {
                if (!this.isResizing) this.maxTicks = null,
                q(b,
                function(a) {
                    a.setScale()
                });
                this.adjustTickAmounts();
                this.getMargins();
                q(b,
                function(a) {
                    a.isDirty && (i = !0)
                });
                q(b,
                function(a) {
                    if (a.isDirtyExtremes) a.isDirtyExtremes = !1,
                    n.push(function() {
                        N(a, "afterSetExtremes", v(a.eventArgs, a.getExtremes()));
                        delete a.eventArgs
                    }); (i || g) && a.redraw()
                })
            }
            i && this.drawChartBox();
            q(c,
            function(a) {
                a.isDirty && a.visible && (!a.isCartesian || a.xAxis) && a.redraw()
            });
            d && d.reset(!0);
            l.draw();
            N(this, "redraw");
            m && this.cloneRenderTo(!0);
            q(n,
            function(a) {
                a.call()
            })
        },
        get: function(a) {
            var b = this.axes,
            c = this.series,
            d, e;
            for (d = 0; d < b.length; d++) if (b[d].options.id === a) return b[d];
            for (d = 0; d < c.length; d++) if (c[d].options.id === a) return c[d];
            for (d = 0; d < c.length; d++) {
                e = c[d].points || [];
                for (b = 0; b < e.length; b++) if (e[b].id === a) return e[b]
            }
            return null
        },
        getAxes: function() {
            var a = this,
            b = this.options,
            c = b.xAxis = ma(b.xAxis || {}),
            b = b.yAxis = ma(b.yAxis || {});
            q(c,
            function(a, b) {
                a.index = b;
                a.isX = !0
            });
            q(b,
            function(a, b) {
                a.index = b
            });
            c = c.concat(b);
            q(c,
            function(b) {
                new L(a, b)
            });
            a.adjustTickAmounts()
        },
        getSelectedPoints: function() {
            var a = [];
            q(this.series,
            function(b) {
                a = a.concat(Cb(b.points || [],
                function(a) {
                    return a.selected
                }))
            });
            return a
        },
        getSelectedSeries: function() {
            return Cb(this.series,
            function(a) {
                return a.selected
            })
        },
        getStacks: function() {
            var a = this;
            q(a.yAxis,
            function(a) {
                if (a.stacks && a.hasVisibleSeries) a.oldStacks = a.stacks
            });
            q(a.series,
            function(b) {
                if (b.options.stacking && (b.visible === !0 || a.options.chart.ignoreHiddenSeries === !1)) b.stackKey = b.type + o(b.options.stack, "")
            })
        },
        setTitle: function(a, b, c) {
            var g;
            var d = this,
            e = d.options,
            f;
            f = e.title = y(e.title, a);
            g = e.subtitle = y(e.subtitle, b),
            e = g;
            q([["title", a, f], ["subtitle", b, e]],
            function(a) {
                var b = a[0],
                c = d[b],
                e = a[1],
                a = a[2];
                c && e && (d[b] = c = c.destroy());
                a && a.text && !c && (d[b] = d.renderer.text(a.text, 0, 0, a.useHTML).attr({
                    align: a.align,
                    "class": "highcharts-" + b,
                    zIndex: a.zIndex || 4
                }).css(a.style).add())
            });
            d.layOutTitles(c)
        },
        layOutTitles: function(a) {
            var b = 0,
            c = this.title,
            d = this.subtitle,
            e = this.options,
            f = e.title,
            e = e.subtitle,
            g = this.spacingBox.width - 44;
            if (c && (c.css({
                width: (f.width || g) + "px"
            }).align(v({
                y: 15
            },
            f), !1, "spacingBox"), !f.floating && !f.verticalAlign)) b = c.getBBox().height;
            d && (d.css({
                width: (e.width || g) + "px"
            }).align(v({
                y: b + f.margin
            },
            e), !1, "spacingBox"), !e.floating && !e.verticalAlign && (b = Va(b + d.getBBox().height)));
            c = this.titleOffset !== b;
            this.titleOffset = b;
            if (!this.isDirtyBox && c) this.isDirtyBox = c,
            this.hasRendered && o(a, !0) && this.isDirtyBox && this.redraw()
        },
        getChartSize: function() {
            var a = this.options.chart,
            b = a.width,
            a = a.height,
            c = this.renderToClone || this.renderTo;
            if (!t(b)) this.containerWidth = nb(c, "width");
            if (!t(a)) this.containerHeight = nb(c, "height");
            this.chartWidth = w(0, b || this.containerWidth || 600);
            this.chartHeight = w(0, o(a, this.containerHeight > 19 ? this.containerHeight: 400))
        },
        cloneRenderTo: function(a) {
            var b = this.renderToClone,
            c = this.container;
            a ? b && (this.renderTo.appendChild(c), Sa(b), delete this.renderToClone) : (c && c.parentNode === this.renderTo && this.renderTo.removeChild(c), this.renderToClone = b = this.renderTo.cloneNode(0), E(b, {
                position: "absolute",
                top: "-9999px",
                display: "block"
            }), b.style.setProperty && b.style.setProperty("display", "block", "important"), B.body.appendChild(b), c && b.appendChild(c))
        },
        getContainer: function() {
            var a, b = this.options.chart,
            c, d, e;
            this.renderTo = a = b.renderTo;
            e = "highcharts-" + Ab++;
            if (Oa(a)) this.renderTo = a = B.getElementById(a);
            a || qa(13, !0);
            c = I(W(a, "data-highcharts-chart")); ! isNaN(c) && aa[c] && aa[c].hasRendered && aa[c].destroy();
            W(a, "data-highcharts-chart", this.index);
            a.innerHTML = ""; ! b.skipClone && !a.offsetWidth && this.cloneRenderTo();
            this.getChartSize();
            c = this.chartWidth;
            d = this.chartHeight;
            this.container = a = $(Ta, {
                className: "highcharts-container" + (b.className ? " " + b.className: ""),
                id: e
            },
            v({
                position: "relative",
                overflow: "hidden",
                width: c + "px",
                height: d + "px",
                textAlign: "left",
                lineHeight: "normal",
                zIndex: 0,
                "-webkit-tap-highlight-color": "rgba(0,0,0,0)"
            },
            b.style), this.renderToClone || a);
            this._cursor = a.style.cursor;
            this.renderer = b.forExport ? new ka(a, c, d, b.style, !0) : new Wa(a, c, d, b.style);
            ja && this.renderer.create(this, a, c, d)
        },
        getMargins: function() {
            var a = this.spacing,
            b, c = this.legend,
            d = this.margin,
            e = this.options.legend,
            f = o(e.margin, 20),
            g = e.x,
            h = e.y,
            i = e.align,
            k = e.verticalAlign,
            j = this.titleOffset;
            this.resetMargins();
            b = this.axisOffset;
            if (j && !t(d[0])) this.plotTop = w(this.plotTop, j + this.options.title.margin + a[0]);
            if (c.display && !e.floating) if (i === "right") {
                if (!t(d[1])) this.marginRight = w(this.marginRight, c.legendWidth - g + f + a[1])
            } else if (i === "left") {
                if (!t(d[3])) this.plotLeft = w(this.plotLeft, c.legendWidth + g + f + a[3])
            } else if (k === "top") {
                if (!t(d[0])) this.plotTop = w(this.plotTop, c.legendHeight + h + f + a[0])
            } else if (k === "bottom" && !t(d[2])) this.marginBottom = w(this.marginBottom, c.legendHeight - h + f + a[2]);
            this.extraBottomMargin && (this.marginBottom += this.extraBottomMargin);
            this.extraTopMargin && (this.plotTop += this.extraTopMargin);
            this.hasCartesianSeries && q(this.axes,
            function(a) {
                a.getOffset()
            });
            t(d[3]) || (this.plotLeft += b[3]);
            t(d[0]) || (this.plotTop += b[0]);
            t(d[2]) || (this.marginBottom += b[2]);
            t(d[1]) || (this.marginRight += b[1]);
            this.setChartSize()
        },
        reflow: function(a) {
            var b = this,
            c = b.options.chart,
            d = b.renderTo,
            e = c.width || nb(d, "width"),
            f = c.height || nb(d, "height"),
            c = a ? a.target: X,
            d = function() {
                if (b.container) b.setSize(e, f, !1),
                b.hasUserSize = null
            };
            if (!b.hasUserSize && e && f && (c === X || c === B)) {
                if (e !== b.containerWidth || f !== b.containerHeight) clearTimeout(b.reflowTimeout),
                a ? b.reflowTimeout = setTimeout(d, 100) : d();
                b.containerWidth = e;
                b.containerHeight = f
            }
        },
        initReflow: function() {
            var a = this,
            b = function(b) {
                a.reflow(b)
            };
            A(X, "resize", b);
            A(a, "destroy",
            function() {
                R(X, "resize", b)
            })
        },
        setSize: function(a, b, c) {
            var d = this,
            e, f, g;
            d.isResizing += 1;
            g = function() {
                d && N(d, "endResize", null,
                function() {
                    d.isResizing -= 1
                })
            };
            Ya(c, d);
            d.oldChartHeight = d.chartHeight;
            d.oldChartWidth = d.chartWidth;
            if (t(a)) d.chartWidth = e = w(0, s(a)),
            d.hasUserSize = !!e;
            if (t(b)) d.chartHeight = f = w(0, s(b)); (Ca ? ob: E)(d.container, {
                width: e + "px",
                height: f + "px"
            },
            Ca);
            d.setChartSize(!0);
            d.renderer.setSize(e, f, c);
            d.maxTicks = null;
            q(d.axes,
            function(a) {
                a.isDirty = !0;
                a.setScale()
            });
            q(d.series,
            function(a) {
                a.isDirty = !0
            });
            d.isDirtyLegend = !0;
            d.isDirtyBox = !0;
            d.layOutTitles();
            d.getMargins();
            d.redraw(c);
            d.oldChartHeight = null;
            N(d, "resize");
            Ca === !1 ? g() : setTimeout(g, Ca && Ca.duration || 500)
        },
        setChartSize: function(a) {
            var b = this.inverted,
            c = this.renderer,
            d = this.chartWidth,
            e = this.chartHeight,
            f = this.options.chart,
            g = this.spacing,
            h = this.clipOffset,
            i, k, j, l;
            this.plotLeft = i = s(this.plotLeft);
            this.plotTop = k = s(this.plotTop);
            this.plotWidth = j = w(0, s(d - i - this.marginRight));
            this.plotHeight = l = w(0, s(e - k - this.marginBottom));
            this.plotSizeX = b ? l: j;
            this.plotSizeY = b ? j: l;
            this.plotBorderWidth = f.plotBorderWidth || 0;
            this.spacingBox = c.spacingBox = {
                x: g[3],
                y: g[0],
                width: d - g[3] - g[1],
                height: e - g[0] - g[2]
            };
            this.plotBox = c.plotBox = {
                x: i,
                y: k,
                width: j,
                height: l
            };
            d = 2 * S(this.plotBorderWidth / 2);
            b = Va(w(d, h[3]) / 2);
            c = Va(w(d, h[0]) / 2);
            this.clipBox = {
                x: b,
                y: c,
                width: S(this.plotSizeX - w(d, h[1]) / 2 - b),
                height: S(this.plotSizeY - w(d, h[2]) / 2 - c)
            };
            a || q(this.axes,
            function(a) {
                a.setAxisSize();
                a.setAxisTranslation()
            })
        },
        resetMargins: function() {
            var a = this.spacing,
            b = this.margin;
            this.plotTop = o(b[0], a[0]);
            this.marginRight = o(b[1], a[1]);
            this.marginBottom = o(b[2], a[2]);
            this.plotLeft = o(b[3], a[3]);
            this.axisOffset = [0, 0, 0, 0];
            this.clipOffset = [0, 0, 0, 0]
        },
        drawChartBox: function() {
            var a = this.options.chart,
            b = this.renderer,
            c = this.chartWidth,
            d = this.chartHeight,
            e = this.chartBackground,
            f = this.plotBackground,
            g = this.plotBorder,
            h = this.plotBGImage,
            i = a.borderWidth || 0,
            k = a.backgroundColor,
            j = a.plotBackgroundColor,
            l = a.plotBackgroundImage,
            m = a.plotBorderWidth || 0,
            n, p = this.plotLeft,
            u = this.plotTop,
            o = this.plotWidth,
            q = this.plotHeight,
            x = this.plotBox,
            r = this.clipRect,
            s = this.clipBox;
            n = i + (a.shadow ? 8 : 0);
            if (i || k) if (e) e.animate(e.crisp({
                width: c - n,
                height: d - n
            }));
            else {
                e = {
                    fill: k || Y
                };
                if (i) e.stroke = a.borderColor,
                e["stroke-width"] = i;
                this.chartBackground = b.rect(n / 2, n / 2, c - n, d - n, a.borderRadius, i).attr(e).addClass("highcharts-background").add().shadow(a.shadow)
            }
            if (j) f ? f.animate(x) : this.plotBackground = b.rect(p, u, o, q, 0).attr({
                fill: j
            }).add().shadow(a.plotShadow);
            if (l) h ? h.animate(x) : this.plotBGImage = b.image(l, p, u, o, q).add();
            r ? r.animate({
                width: s.width,
                height: s.height
            }) : this.clipRect = b.clipRect(s);
            if (m) g ? g.animate(g.crisp({
                x: p,
                y: u,
                width: o,
                height: q
            })) : this.plotBorder = b.rect(p, u, o, q, 0, -m).attr({
                stroke: a.plotBorderColor,
                "stroke-width": m,
                fill: Y,
                zIndex: 1
            }).add();
            this.isDirtyBox = !1
        },
        propFromSeries: function() {
            var a = this,
            b = a.options.chart,
            c, d = a.options.series,
            e, f;
            q(["inverted", "angular", "polar"],
            function(g) {
                c = C[b.type || b.defaultSeriesType];
                f = a[g] || b[g] || c && c.prototype[g];
                for (e = d && d.length; ! f && e--;)(c = C[d[e].type]) && c.prototype[g] && (f = !0);
                a[g] = f
            })
        },
        linkSeries: function() {
            var a = this,
            b = a.series;
            q(b,
            function(a) {
                a.linkedSeries.length = 0
            });
            q(b,
            function(b) {
                var d = b.options.linkedTo;
                if (Oa(d) && (d = d === ":previous" ? a.series[b.index - 1] : a.get(d))) d.linkedSeries.push(b),
                b.linkedParent = d
            })
        },
        renderSeries: function() {
            q(this.series,
            function(a) {
                a.translate();
                a.setTooltipPoints && a.setTooltipPoints();
                a.render()
            })
        },
        render: function() {
            var a = this,
            b = a.axes,
            c = a.renderer,
            d = a.options,
            e = d.labels,
            f = d.credits,
            g;
            a.setTitle();
            a.legend = new pb(a, d.legend);
            a.getStacks();
            q(b,
            function(a) {
                a.setScale()
            });
            a.getMargins();
            a.maxTicks = null;
            q(b,
            function(a) {
                a.setTickPositions(!0);
                a.setMaxTicks()
            });
            a.adjustTickAmounts();
            a.getMargins();
            a.drawChartBox();
            a.hasCartesianSeries && q(b,
            function(a) {
                a.render()
            });
            if (!a.seriesGroup) a.seriesGroup = c.g("series-group").attr({
                zIndex: 3
            }).add();
            a.renderSeries();
            e.items && q(e.items,
            function(b) {
                var d = v(e.style, b.style),
                f = I(d.left) + a.plotLeft,
                g = I(d.top) + a.plotTop + 12;
                delete d.left;
                delete d.top;
                c.text(b.html, f, g).attr({
                    zIndex: 2
                }).css(d).add()
            });
            if (f.enabled && !a.credits) g = f.href,
            a.credits = c.text(f.text, 0, 0).on("click",
            function() {
                if (g) location.href = g
            }).attr({
                align: f.position.align,
                zIndex: 8
            }).css(f.style).add().align(f.position);
            a.hasRendered = !0
        },
        destroy: function() {
            var a = this,
            b = a.axes,
            c = a.series,
            d = a.container,
            e, f = d && d.parentNode;
            N(a, "destroy");
            aa[a.index] = r;
            db--;
            a.renderTo.removeAttribute("data-highcharts-chart");
            R(a);
            for (e = b.length; e--;) b[e] = b[e].destroy();
            for (e = c.length; e--;) c[e] = c[e].destroy();
            q("title,subtitle,chartBackground,plotBackground,plotBGImage,plotBorder,seriesGroup,clipRect,credits,pointer,scroller,rangeSelector,legend,resetZoomButton,tooltip,renderer".split(","),
            function(b) {
                var c = a[b];
                c && c.destroy && (a[b] = c.destroy())
            });
            if (d) d.innerHTML = "",
            R(d),
            f && Sa(d);
            for (e in a) delete a[e]
        },
        isReadyToRender: function() {
            var a = this;
            return ! ca && X == X.top && B.readyState !== "complete" || ja && !X.canvg ? (ja ? Vb.push(function() {
                a.firstRender()
            },
            a.options.global.canvasToolsURL) : B.attachEvent("onreadystatechange",
            function() {
                B.detachEvent("onreadystatechange", a.firstRender);
                B.readyState === "complete" && a.firstRender()
            }), !1) : !0
        },
        firstRender: function() {
            var a = this,
            b = a.options,
            c = a.callback;
            if (a.isReadyToRender()) {
                a.getContainer();
                N(a, "init");
                a.resetMargins();
                a.setChartSize();
                a.propFromSeries();
                a.getAxes();
                q(b.series || [],
                function(b) {
                    a.initSeries(b)
                });
                a.linkSeries();
                N(a, "beforeRender");
                if (P.Pointer) a.pointer = new Xa(a, b);
                a.render();
                a.renderer.draw();
                c && c.apply(a, [a]);
                q(a.callbacks,
                function(b) {
                    b.apply(a, [a])
                });
                a.cloneRenderTo(!0);
                N(a, "load")
            }
        },
        splashArray: function(a, b) {
            var c = b[a],
            c = fa(c) ? c: [c, c, c, c];
            return [o(b[a + "Top"], c[0]), o(b[a + "Right"], c[1]), o(b[a + "Bottom"], c[2]), o(b[a + "Left"], c[3])]
        }
    };
    va.prototype.callbacks = [];
    ea = P.CenteredSeriesMixin = {
        getCenter: function() {
            var a = this.options,
            b = this.chart,
            c = 2 * (a.slicedOffset || 0),
            d,
            e = b.plotWidth - 2 * c,
            f = b.plotHeight - 2 * c,
            b = a.center,
            a = [o(b[0], "50%"), o(b[1], "50%"), a.size || "100%", a.innerSize || 0],
            g = z(e, f),
            h;
            return xa(a,
            function(a, b) {
                h = /%$/.test(a);
                d = b < 2 || b === 2 && h;
                return (h ? [e, f, g, g][b] * I(a) / 100 : a) + (d ? c: 0)
            })
        }
    };
    var za = function() {};
    za.prototype = {
        init: function(a, b, c) {
            this.series = a;
            this.applyOptions(b, c);
            this.pointAttr = {};
            if (a.options.colorByPoint && (b = a.options.colors || a.chart.options.colors, this.color = this.color || b[a.colorCounter++], a.colorCounter === b.length)) a.colorCounter = 0;
            a.chart.pointCount++;
            return this
        },
        applyOptions: function(a, b) {
            var c = this.series,
            d = c.pointValKey,
            a = za.prototype.optionsToObject.call(this, a);
            v(this, a);
            this.options = this.options ? v(this.options, a) : a;
            if (d) this.y = this[d];
            if (this.x === r && c) this.x = b === r ? c.autoIncrement() : b;
            return this
        },
        optionsToObject: function(a) {
            var b = {},
            c = this.series,
            d = c.pointArrayMap || ["y"],
            e = d.length,
            f = 0,
            g = 0;
            if (typeof a === "number" || a === null) b[d[0]] = a;
            else if (Pa(a)) {
                if (a.length > e) {
                    c = typeof a[0];
                    if (c === "string") b.name = a[0];
                    else if (c === "number") b.x = a[0];
                    f++
                }
                for (; g < e;) b[d[g++]] = a[f++]
            } else if (typeof a === "object") {
                b = a;
                if (a.dataLabels) c._hasPointLabels = !0;
                if (a.marker) c._hasPointMarkers = !0
            }
            return b
        },
        destroy: function() {
            var a = this.series.chart,
            b = a.hoverPoints,
            c;
            a.pointCount--;
            if (b && (this.setState(), pa(b, this), !b.length)) a.hoverPoints = null;
            if (this === a.hoverPoint) this.onMouseOut();
            if (this.graphic || this.dataLabel) R(this),
            this.destroyElements();
            this.legendItem && a.legend.destroyItem(this);
            for (c in this) this[c] = null
        },
        destroyElements: function() {
            for (var a = "graphic,dataLabel,dataLabelUpper,group,connector,shadowGroup".split(","), b, c = 6; c--;) b = a[c],
            this[b] && (this[b] = this[b].destroy())
        },
        getLabelConfig: function() {
            return {
                x: this.category,
                y: this.y,
                key: this.name || this.category,
                series: this.series,
                point: this,
                percentage: this.percentage,
                total: this.total || this.stackTotal
            }
        },
        tooltipFormatter: function(a) {
            var b = this.series,
            c = b.tooltipOptions,
            d = o(c.valueDecimals, ""),
            e = c.valuePrefix || "",
            f = c.valueSuffix || "";
            q(b.pointArrayMap || ["y"],
            function(b) {
                b = "{point." + b;
                if (e || f) a = a.replace(b + "}", e + b + "}" + f);
                a = a.replace(b + "}", b + ":,." + d + "f}")
            });
            return Ja(a, {
                point: this,
                series: this.series
            })
        },
        firePointEvent: function(a, b, c) {
            var d = this,
            e = this.series.options; (e.point.events[a] || d.options && d.options.events && d.options.events[a]) && this.importEvents();
            a === "click" && e.allowPointSelect && (c = function(a) {
                d.select(null, a.ctrlKey || a.metaKey || a.shiftKey)
            });
            N(this, a, b, c)
        }
    };
    var K = function() {};
    K.prototype = {
        isCartesian: !0,
        type: "line",
        pointClass: za,
        sorted: !0,
        requireSorting: !0,
        pointAttrToOptions: {
            stroke: "lineColor",
            "stroke-width": "lineWidth",
            fill: "fillColor",
            r: "radius"
        },
        axisTypes: ["xAxis", "yAxis"],
        colorCounter: 0,
        parallelArrays: ["x", "y"],
        init: function(a, b) {
            var c = this,
            d, e, f = a.series,
            g = function(a, b) {
                return o(a.options.index, a._i) - o(b.options.index, b._i)
            };
            c.chart = a;
            c.options = b = c.setOptions(b);
            c.linkedSeries = [];
            c.bindAxes();
            v(c, {
                name: b.name,
                state: "",
                pointAttr: {},
                visible: b.visible !== !1,
                selected: b.selected === !0
            });
            if (ja) b.animation = !1;
            e = b.events;
            for (d in e) A(c, d, e[d]);
            if (e && e.click || b.point && b.point.events && b.point.events.click || b.allowPointSelect) a.runTrackerClick = !0;
            c.getColor();
            c.getSymbol();
            q(c.parallelArrays,
            function(a) {
                c[a + "Data"] = []
            });
            c.setData(b.data, !1);
            if (c.isCartesian) a.hasCartesianSeries = !0;
            f.push(c);
            c._i = f.length - 1;
            tb(f, g);
            this.yAxis && tb(this.yAxis.series, g);
            q(f,
            function(a, b) {
                a.index = b;
                a.name = a.name || "Series " + (b + 1)
            })
        },
        bindAxes: function() {
            var a = this,
            b = a.options,
            c = a.chart,
            d;
            q(a.axisTypes || [],
            function(e) {
                q(c[e],
                function(c) {
                    d = c.options;
                    if (b[e] === d.index || b[e] !== r && b[e] === d.id || b[e] === r && d.index === 0) c.series.push(a),
                    a[e] = c,
                    c.isDirty = !0
                }); ! a[e] && a.optionalAxis !== e && qa(18, !0)
            })
        },
        updateParallelArrays: function(a, b) {
            var c = a.series,
            d = arguments;
            q(c.parallelArrays, typeof b === "number" ?
            function(d) {
                var f = d === "y" && c.toYData ? c.toYData(a) : a[d];
                c[d + "Data"][b] = f
            }: function(a) {
                Array.prototype[b].apply(c[a + "Data"], Array.prototype.slice.call(d, 2))
            })
        },
        autoIncrement: function() {
            var a = this.options,
            b = this.xIncrement,
            b = o(b, a.pointStart, 0);
            this.pointInterval = o(this.pointInterval, a.pointInterval, 1);
            this.xIncrement = b + this.pointInterval;
            return b
        },
        getSegments: function() {
            var a = -1,
            b = [],
            c,
            d = this.points,
            e = d.length;
            if (e) if (this.options.connectNulls) {
                for (c = e; c--;) d[c].y === null && d.splice(c, 1);
                d.length && (b = [d])
            } else q(d,
            function(c, g) {
                c.y === null ? (g > a + 1 && b.push(d.slice(a + 1, g)), a = g) : g === e - 1 && b.push(d.slice(a + 1, g + 1))
            });
            this.segments = b
        },
        setOptions: function(a) {
            var b = this.chart,
            c = b.options.plotOptions,
            b = b.userOptions || {},
            d = b.plotOptions || {},
            e = c[this.type];
            this.userOptions = a;
            c = y(e, c.series, a);
            this.tooltipOptions = y(F.tooltip, F.plotOptions[this.type].tooltip, b.tooltip, d.series && d.series.tooltip, d[this.type] && d[this.type].tooltip, a.tooltip);
            e.marker === null && delete c.marker;
            return c
        },
        getColor: function() {
            var a = this.options,
            b = this.userOptions,
            c = this.chart.options.colors,
            d = this.chart.counters,
            e;
            e = a.color || T[this.type].color;
            if (!e && !a.colorByPoint) t(b._colorIndex) ? a = b._colorIndex: (b._colorIndex = d.color, a = d.color++),
            e = c[a];
            this.color = e;
            d.wrapColor(c.length)
        },
        getSymbol: function() {
            var a = this.userOptions,
            b = this.options.marker,
            c = this.chart,
            d = c.options.symbols,
            c = c.counters;
            this.symbol = b.symbol;
            if (!this.symbol) t(a._symbolIndex) ? a = a._symbolIndex: (a._symbolIndex = c.symbol, a = c.symbol++),
            this.symbol = d[a];
            if (/^url/.test(this.symbol)) b.radius = 0;
            c.wrapSymbol(d.length)
        },
        drawLegendSymbol: Q.drawLineMarker,
        setData: function(a, b, c, d) {
            var e = this,
            f = e.points,
            g = f && f.length || 0,
            h, i = e.options,
            k = e.chart,
            j = null,
            l = e.xAxis,
            m = l && !!l.categories,
            n = e.tooltipPoints,
            p = i.turboThreshold,
            u = this.xData,
            s = this.yData,
            G = (h = e.pointArrayMap) && h.length,
            a = a || [];
            h = a.length;
            b = o(b, !0);
            if (d !== !1 && h && g === h && !e.cropped && !e.hasGroupedData) q(a,
            function(a, b) {
                f[b].update(a, !1)
            });
            else {
                e.xIncrement = null;
                e.pointRange = m ? 1 : i.pointRange;
                e.colorCounter = 0;
                q(this.parallelArrays,
                function(a) {
                    e[a + "Data"].length = 0
                });
                if (p && h > p) {
                    for (c = 0; j === null && c < h;) j = a[c],
                    c++;
                    if (la(j)) {
                        m = o(i.pointStart, 0);
                        i = o(i.pointInterval, 1);
                        for (c = 0; c < h; c++) u[c] = m,
                        s[c] = a[c],
                        m += i;
                        e.xIncrement = m
                    } else if (Pa(j)) if (G) for (c = 0; c < h; c++) i = a[c],
                    u[c] = i[0],
                    s[c] = i.slice(1, G + 1);
                    else for (c = 0; c < h; c++) i = a[c],
                    u[c] = i[0],
                    s[c] = i[1];
                    else qa(12)
                } else for (c = 0; c < h; c++) if (a[c] !== r && (i = {
                    series: e
                },
                e.pointClass.prototype.applyOptions.apply(i, [a[c]]), e.updateParallelArrays(i, c), m && i.name)) l.names[i.x] = i.name;
                Oa(s[0]) && qa(14, !0);
                e.data = [];
                e.options.data = a;
                for (c = g; c--;) f[c] && f[c].destroy && f[c].destroy();
                if (n) n.length = 0;
                if (l) l.minRange = l.userMinRange;
                e.isDirty = e.isDirtyData = k.isDirtyBox = !0;
                c = !1
            }
            b && k.redraw(c)
        },
        processData: function(a) {
            var b = this.xData,
            c = this.yData,
            d = b.length,
            e;
            e = 0;
            var f, g, h = this.xAxis,
            i = this.options,
            k = i.cropThreshold,
            j = 0,
            l = this.isCartesian,
            m, n;
            if (l && !this.isDirty && !h.isDirty && !this.yAxis.isDirty && !a) return ! 1;
            if (l && this.sorted && (!k || d > k || this.forceCrop)) if (m = h.min, n = h.max, b[d - 1] < m || b[0] > n) b = [],
            c = [];
            else if (b[0] < m || b[d - 1] > n) e = this.cropData(this.xData, this.yData, m, n),
            b = e.xData,
            c = e.yData,
            e = e.start,
            f = !0,
            j = b.length;
            for (d = b.length - 1; d >= 0; d--) a = b[d] - b[d - 1],
            !f && b[d] > m && b[d] < n && j++,
            a > 0 && (g === r || a < g) ? g = a: a < 0 && this.requireSorting && qa(15);
            this.cropped = f;
            this.cropStart = e;
            this.processedXData = b;
            this.processedYData = c;
            this.activePointCount = j;
            if (i.pointRange === null) this.pointRange = g || 1;
            this.closestPointRange = g
        },
        cropData: function(a, b, c, d) {
            var e = a.length,
            f = 0,
            g = e,
            h = o(this.cropShoulder, 1),
            i;
            for (i = 0; i < e; i++) if (a[i] >= c) {
                f = w(0, i - h);
                break
            }
            for (; i < e; i++) if (a[i] > d) {
                g = i + h;
                break
            }
            return {
                xData: a.slice(f, g),
                yData: b.slice(f, g),
                start: f,
                end: g
            }
        },
        generatePoints: function() {
            var a = this.options.data,
            b = this.data,
            c, d = this.processedXData,
            e = this.processedYData,
            f = this.pointClass,
            g = d.length,
            h = this.cropStart || 0,
            i, k = this.hasGroupedData,
            j, l = [],
            m;
            if (!b && !k) b = [],
            b.length = a.length,
            b = this.data = b;
            for (m = 0; m < g; m++) i = h + m,
            k ? l[m] = (new f).init(this, [d[m]].concat(ma(e[m]))) : (b[i] ? j = b[i] : a[i] !== r && (b[i] = j = (new f).init(this, a[i], d[m])), l[m] = j);
            if (b && (g !== (c = b.length) || k)) for (m = 0; m < c; m++) if (m === h && !k && (m += g), b[m]) b[m].destroyElements(),
            b[m].plotX = r;
            this.data = b;
            this.points = l
        },
        getExtremes: function(a) {
            var b = this.yAxis,
            c = this.processedXData,
            d, e = [],
            f = 0;
            d = this.xAxis.getExtremes();
            var g = d.min,
            h = d.max,
            i, k, j, l, a = a || this.stackedYData || this.processedYData;
            d = a.length;
            for (l = 0; l < d; l++) if (k = c[l], j = a[l], i = j !== null && j !== r && (!b.isLog || j.length || j > 0), k = this.getExtremesFromAll || this.cropped || (c[l + 1] || k) >= g && (c[l - 1] || k) <= h, i && k) if (i = j.length) for (; i--;) j[i] !== null && (e[f++] = j[i]);
            else e[f++] = j;
            this.dataMin = o(void 0, Ra(e));
            this.dataMax = o(void 0, Ba(e))
        },
        translate: function() {
            this.processedXData || this.processData();
            this.generatePoints();
            for (var a = this.options,
            b = a.stacking,
            c = this.xAxis,
            d = c.categories,
            e = this.yAxis,
            f = this.points,
            g = f.length,
            h = !!this.modifyValue,
            i = a.pointPlacement,
            k = i === "between" || la(i), j = a.threshold, a = 0; a < g; a++) {
                var l = f[a],
                m = l.x,
                n = l.y,
                p = l.low,
                u = b && e.stacks[(this.negStacks && n < j ? "-": "") + this.stackKey];
                if (e.isLog && n <= 0) l.y = n = null;
                l.plotX = c.translate(m, 0, 0, 0, 1, i, this.type === "flags");
                if (b && this.visible && u && u[m]) u = u[m],
                n = u.points[this.index + "," + a],
                p = n[0],
                n = n[1],
                p === 0 && (p = o(j, e.min)),
                e.isLog && p <= 0 && (p = null),
                l.total = l.stackTotal = u.total,
                l.percentage = u.total && l.y / u.total * 100,
                l.stackY = n,
                u.setOffset(this.pointXOffset || 0, this.barW || 0);
                l.yBottom = t(p) ? e.translate(p, 0, 1, 0, 1) : null;
                h && (n = this.modifyValue(n, l));
                l.plotY = typeof n === "number" && n !== Infinity ? e.translate(n, 0, 1, 0, 1) : r;
                l.clientX = k ? c.translate(m, 0, 0, 0, 1) : l.plotX;
                l.negative = l.y < (j || 0);
                l.category = d && d[l.x] !== r ? d[l.x] : l.x
            }
            this.getSegments()
        },
        animate: function(a) {
            var b = this.chart,
            c = b.renderer,
            d;
            d = this.options.animation;
            var e = this.clipBox || b.clipBox,
            f = b.inverted,
            g;
            if (d && !fa(d)) d = T[this.type].animation;
            g = ["_sharedClip", d.duration, d.easing, e.height].join(",");
            a ? (a = b[g], d = b[g + "m"], a || (b[g] = a = c.clipRect(v(e, {
                width: 0
            })), b[g + "m"] = d = c.clipRect( - 99, f ? -b.plotLeft: -b.plotTop, 99, f ? b.chartWidth: b.chartHeight)), this.group.clip(a), this.markerGroup.clip(d), this.sharedClipKey = g) : ((a = b[g]) && a.animate({
                width: b.plotSizeX
            },
            d), b[g + "m"] && b[g + "m"].animate({
                width: b.plotSizeX + 99
            },
            d), this.animate = null)
        },
        afterAnimate: function() {
            var a = this.chart,
            b = this.sharedClipKey,
            c = this.group,
            d = this.clipBox;
            if (c && this.options.clip !== !1) {
                if (!b || !d) c.clip(d ? a.renderer.clipRect(d) : a.clipRect);
                this.markerGroup.clip()
            }
            N(this, "afterAnimate");
            setTimeout(function() {
                b && a[b] && (d || (a[b] = a[b].destroy()), a[b + "m"] && (a[b + "m"] = a[b + "m"].destroy()))
            },
            100)
        },
        drawPoints: function() {
            var a, b = this.points,
            c = this.chart,
            d, e, f, g, h, i, k, j;
            d = this.options.marker;
            var l = this.pointAttr[""],
            m,
            n = this.markerGroup,
            p = o(d.enabled, this.activePointCount < 0.5 * this.xAxis.len / d.radius);
            if (d.enabled !== !1 || this._hasPointMarkers) for (f = b.length; f--;) if (g = b[f], d = S(g.plotX), e = g.plotY, j = g.graphic, i = g.marker || {},
            a = p && i.enabled === r || i.enabled, m = c.isInsidePlot(s(d), e, c.inverted), a && e !== r && !isNaN(e) && g.y !== null) if (a = g.pointAttr[g.selected ? "select": ""] || l, h = a.r, i = o(i.symbol, this.symbol), k = i.indexOf("url") === 0, j) j[m ? "show": "hide"](!0).animate(v({
                x: d - h,
                y: e - h
            },
            j.symbolName ? {
                width: 2 * h,
                height: 2 * h
            }: {}));
            else {
                if (m && (h > 0 || k)) g.graphic = c.renderer.symbol(i, d - h, e - h, 2 * h, 2 * h).attr(a).add(n)
            } else if (j) g.graphic = j.destroy()
        },
        convertAttribs: function(a, b, c, d) {
            var e = this.pointAttrToOptions,
            f, g, h = {},
            a = a || {},
            b = b || {},
            c = c || {},
            d = d || {};
            for (f in e) g = e[f],
            h[f] = o(a[g], b[f], c[f], d[f]);
            return h
        },
        getAttribs: function() {
            var a = this,
            b = a.options,
            c = T[a.type].marker ? b.marker: b,
            d = c.states,
            e = d.hover,
            f,
            g = a.color;
            f = {
                stroke: g,
                fill: g
            };
            var h = a.points || [],
            i,
            k = [],
            j,
            l = a.pointAttrToOptions;
            j = a.hasPointSpecificOptions;
            var m = b.negativeColor,
            n = c.lineColor,
            p = c.fillColor;
            i = b.turboThreshold;
            var u;
            b.marker ? (e.radius = e.radius || c.radius + 2, e.lineWidth = e.lineWidth || c.lineWidth + 1) : e.color = e.color || Fa(e.color || g).brighten(e.brightness).get();
            k[""] = a.convertAttribs(c, f);
            q(["hover", "select"],
            function(b) {
                k[b] = a.convertAttribs(d[b], k[""])
            });
            a.pointAttr = k;
            g = h.length;
            if (!i || g < i || j) for (; g--;) {
                i = h[g];
                if ((c = i.options && i.options.marker || i.options) && c.enabled === !1) c.radius = 0;
                if (i.negative && m) i.color = i.fillColor = m;
                j = b.colorByPoint || i.color;
                if (i.options) for (u in l) t(c[l[u]]) && (j = !0);
                if (j) {
                    c = c || {};
                    j = [];
                    d = c.states || {};
                    f = d.hover = d.hover || {};
                    if (!b.marker) f.color = f.color || !i.options.color && e.color || Fa(i.color).brighten(f.brightness || e.brightness).get();
                    f = {
                        color: i.color
                    };
                    if (!p) f.fillColor = i.color;
                    if (!n) f.lineColor = i.color;
                    j[""] = a.convertAttribs(v(f, c), k[""]);
                    j.hover = a.convertAttribs(d.hover, k.hover, j[""]);
                    j.select = a.convertAttribs(d.select, k.select, j[""])
                } else j = k;
                i.pointAttr = j
            }
        },
        destroy: function() {
            var a = this,
            b = a.chart,
            c = /AppleWebKit\/533/.test(Da),
            d,
            e,
            f = a.data || [],
            g,
            h,
            i;
            N(a, "destroy");
            R(a);
            q(a.axisTypes || [],
            function(b) {
                if (i = a[b]) pa(i.series, a),
                i.isDirty = i.forceRedraw = !0
            });
            a.legendItem && a.chart.legend.destroyItem(a);
            for (e = f.length; e--;)(g = f[e]) && g.destroy && g.destroy();
            a.points = null;
            clearTimeout(a.animationTimeout);
            q("area,graph,dataLabelsGroup,group,markerGroup,tracker,graphNeg,areaNeg,posClip,negClip".split(","),
            function(b) {
                a[b] && (d = c && b === "group" ? "hide": "destroy", a[b][d]())
            });
            if (b.hoverSeries === a) b.hoverSeries = null;
            pa(b.series, a);
            for (h in a) delete a[h]
        },
        getSegmentPath: function(a) {
            var b = this,
            c = [],
            d = b.options.step;
            q(a,
            function(e, f) {
                var g = e.plotX,
                h = e.plotY,
                i;
                b.getPointSpline ? c.push.apply(c, b.getPointSpline(a, e, f)) : (c.push(f ? "L": "M"), d && f && (i = a[f - 1], d === "right" ? c.push(i.plotX, h) : d === "center" ? c.push((i.plotX + g) / 2, i.plotY, (i.plotX + g) / 2, h) : c.push(g, i.plotY)), c.push(e.plotX, e.plotY))
            });
            return c
        },
        getGraphPath: function() {
            var a = this,
            b = [],
            c,
            d = [];
            q(a.segments,
            function(e) {
                c = a.getSegmentPath(e);
                e.length > 1 ? b = b.concat(c) : d.push(e[0])
            });
            a.singlePoints = d;
            return a.graphPath = b
        },
        drawGraph: function() {
            var a = this,
            b = this.options,
            c = [["graph", b.lineColor || this.color]],
            d = b.lineWidth,
            e = b.dashStyle,
            f = b.linecap !== "square",
            g = this.getGraphPath(),
            h = b.negativeColor;
            h && c.push(["graphNeg", h]);
            q(c,
            function(c, h) {
                var j = c[0],
                l = a[j];
                if (l) eb(l),
                l.animate({
                    d: g
                });
                else if (d && g.length) l = {
                    stroke: c[1],
                    "stroke-width": d,
                    fill: Y,
                    zIndex: 1
                },
                e ? l.dashstyle = e: f && (l["stroke-linecap"] = l["stroke-linejoin"] = "round"),
                a[j] = a.chart.renderer.path(g).attr(l).add(a.group).shadow(!h && b.shadow)
            })
        },
        clipNeg: function() {
            var a = this.options,
            b = this.chart,
            c = b.renderer,
            d = a.negativeColor || a.negativeFillColor,
            e, f = this.graph,
            g = this.area,
            h = this.posClip,
            i = this.negClip;
            e = b.chartWidth;
            var k = b.chartHeight,
            j = w(e, k),
            l = this.yAxis;
            if (d && (f || g)) {
                d = s(l.toPixels(a.threshold || 0, !0));
                d < 0 && (j -= d);
                a = {
                    x: 0,
                    y: 0,
                    width: j,
                    height: d
                };
                j = {
                    x: 0,
                    y: d,
                    width: j,
                    height: j
                };
                if (b.inverted) a.height = j.y = b.plotWidth - d,
                c.isVML && (a = {
                    x: b.plotWidth - d - b.plotLeft,
                    y: 0,
                    width: e,
                    height: k
                },
                j = {
                    x: d + b.plotLeft - e,
                    y: 0,
                    width: b.plotLeft + d,
                    height: e
                });
                l.reversed ? (b = j, e = a) : (b = a, e = j);
                h ? (h.animate(b), i.animate(e)) : (this.posClip = h = c.clipRect(b), this.negClip = i = c.clipRect(e), f && this.graphNeg && (f.clip(h), this.graphNeg.clip(i)), g && (g.clip(h), this.areaNeg.clip(i)))
            }
        },
        invertGroups: function() {
            function a() {
                var a = {
                    width: b.yAxis.len,
                    height: b.xAxis.len
                };
                q(["group", "markerGroup"],
                function(c) {
                    b[c] && b[c].attr(a).invert()
                })
            }
            var b = this,
            c = b.chart;
            if (b.xAxis) A(c, "resize", a),
            A(b, "destroy",
            function() {
                R(c, "resize", a)
            }),
            a(),
            b.invertGroups = a
        },
        plotGroup: function(a, b, c, d, e) {
            var f = this[a],
            g = !f;
            g && (this[a] = f = this.chart.renderer.g(b).attr({
                visibility: c,
                zIndex: d || 0.1
            }).add(e));
            f[g ? "attr": "animate"](this.getPlotBox());
            return f
        },
        getPlotBox: function() {
            var a = this.chart,
            b = this.xAxis,
            c = this.yAxis;
            if (a.inverted) b = c,
            c = this.xAxis;
            return {
                translateX: b ? b.left: a.plotLeft,
                translateY: c ? c.top: a.plotTop,
                scaleX: 1,
                scaleY: 1
            }
        },
        render: function() {
            var a = this,
            b = a.chart,
            c, d = a.options,
            e = (c = d.animation) && !!a.animate && b.renderer.isSVG && o(c.duration, 500) || 0,
            f = a.visible ? "visible": "hidden",
            g = d.zIndex,
            h = a.hasRendered,
            i = b.seriesGroup;
            c = a.plotGroup("group", "series", f, g, i);
            a.markerGroup = a.plotGroup("markerGroup", "markers", f, g, i);
            e && a.animate(!0);
            a.getAttribs();
            c.inverted = a.isCartesian ? b.inverted: !1;
            a.drawGraph && (a.drawGraph(), a.clipNeg());
            a.drawDataLabels && a.drawDataLabels();
            a.visible && a.drawPoints();
            a.drawTracker && a.options.enableMouseTracking !== !1 && a.drawTracker();
            b.inverted && a.invertGroups();
            d.clip !== !1 && !a.sharedClipKey && !h && c.clip(b.clipRect);
            e && a.animate();
            if (!h) e ? a.animationTimeout = setTimeout(function() {
                a.afterAnimate()
            },
            e) : a.afterAnimate();
            a.isDirty = a.isDirtyData = !1;
            a.hasRendered = !0
        },
        redraw: function() {
            var a = this.chart,
            b = this.isDirtyData,
            c = this.group,
            d = this.xAxis,
            e = this.yAxis;
            c && (a.inverted && c.attr({
                width: a.plotWidth,
                height: a.plotHeight
            }), c.animate({
                translateX: o(d && d.left, a.plotLeft),
                translateY: o(e && e.top, a.plotTop)
            }));
            this.translate();
            this.setTooltipPoints && this.setTooltipPoints(!0);
            this.render();
            b && N(this, "updatedData")
        }
    };
    Rb.prototype = {
        destroy: function() {
            Ka(this, this.axis)
        },
        render: function(a) {
            var b = this.options,
            c = b.format,
            c = c ? Ja(c, this) : b.formatter.call(this);
            this.label ? this.label.attr({
                text: c,
                visibility: "hidden"
            }) : this.label = this.axis.chart.renderer.text(c, null, null, b.useHTML).css(b.style).attr({
                align: this.textAlign,
                rotation: b.rotation,
                visibility: "hidden"
            }).add(a)
        },
        setOffset: function(a, b) {
            var c = this.axis,
            d = c.chart,
            e = d.inverted,
            f = this.isNegative,
            g = c.translate(c.usePercentage ? 100 : this.total, 0, 0, 0, 1),
            c = c.translate(0),
            c = M(g - c),
            h = d.xAxis[0].translate(this.x) + a,
            i = d.plotHeight,
            f = {
                x: e ? f ? g: g - c: h,
                y: e ? i - h - b: f ? i - g - c: i - g,
                width: e ? c: b,
                height: e ? b: c
            };
            if (e = this.label) e.align(this.alignOptions, null, f),
            f = e.alignAttr,
            e[this.options.crop === !1 || d.isInsidePlot(f.x, f.y) ? "show": "hide"](!0)
        }
    };
    L.prototype.buildStacks = function() {
        var a = this.series,
        b = o(this.options.reversedStacks, !0),
        c = a.length;
        if (!this.isXAxis) {
            for (this.usePercentage = !1; c--;) a[b ? c: a.length - c - 1].setStackedPoints();
            if (this.usePercentage) for (c = 0; c < a.length; c++) a[c].setPercentStacks()
        }
    };
    L.prototype.renderStackTotals = function() {
        var a = this.chart,
        b = a.renderer,
        c = this.stacks,
        d, e, f = this.stackTotalGroup;
        if (!f) this.stackTotalGroup = f = b.g("stack-labels").attr({
            visibility: "visible",
            zIndex: 6
        }).add();
        f.translate(a.plotLeft, a.plotTop);
        for (d in c) for (e in a = c[d], a) a[e].render(f)
    };
    K.prototype.setStackedPoints = function() {
        if (this.options.stacking && !(this.visible !== !0 && this.chart.options.chart.ignoreHiddenSeries !== !1)) {
            var a = this.processedXData,
            b = this.processedYData,
            c = [],
            d = b.length,
            e = this.options,
            f = e.threshold,
            g = e.stack,
            e = e.stacking,
            h = this.stackKey,
            i = "-" + h,
            k = this.negStacks,
            j = this.yAxis,
            l = j.stacks,
            m = j.oldStacks,
            n,
            p,
            u,
            o,
            q,
            x;
            for (o = 0; o < d; o++) {
                q = a[o];
                x = b[o];
                u = this.index + "," + o;
                p = (n = k && x < f) ? i: h;
                l[p] || (l[p] = {});
                if (!l[p][q]) m[p] && m[p][q] ? (l[p][q] = m[p][q], l[p][q].total = null) : l[p][q] = new Rb(j, j.options.stackLabels, n, q, g);
                p = l[p][q];
                p.points[u] = [p.cum || 0];
                e === "percent" ? (n = n ? h: i, k && l[n] && l[n][q] ? (n = l[n][q], p.total = n.total = w(n.total, p.total) + M(x) || 0) : p.total = ha(p.total + (M(x) || 0))) : p.total = ha(p.total + (x || 0));
                p.cum = (p.cum || 0) + (x || 0);
                p.points[u].push(p.cum);
                c[o] = p.cum
            }
            if (e === "percent") j.usePercentage = !0;
            this.stackedYData = c;
            j.oldStacks = {}
        }
    };
    K.prototype.setPercentStacks = function() {
        var a = this,
        b = a.stackKey,
        c = a.yAxis.stacks,
        d = a.processedXData;
        q([b, "-" + b],
        function(b) {
            var e;
            for (var f = d.length,
            g, h; f--;) if (g = d[f], e = (h = c[b] && c[b][g]) && h.points[a.index + "," + f], g = e) h = h.total ? 100 / h.total: 0,
            g[0] = ha(g[0] * h),
            g[1] = ha(g[1] * h),
            a.stackedYData[f] = g[1]
        })
    };
    v(va.prototype, {
        addSeries: function(a, b, c) {
            var d, e = this;
            a && (b = o(b, !0), N(e, "addSeries", {
                options: a
            },
            function() {
                d = e.initSeries(a);
                e.isDirtyLegend = !0;
                e.linkSeries();
                b && e.redraw(c)
            }));
            return d
        },
        addAxis: function(a, b, c, d) {
            var e = b ? "xAxis": "yAxis",
            f = this.options;
            new L(this, y(a, {
                index: this[e].length,
                isX: b
            }));
            f[e] = ma(f[e] || {});
            f[e].push(a);
            o(c, !0) && this.redraw(d)
        },
        showLoading: function(a) {
            var b = this.options,
            c = this.loadingDiv,
            d = b.loading;
            if (!c) this.loadingDiv = c = $(Ta, {
                className: "highcharts-loading"
            },
            v(d.style, {
                zIndex: 10,
                display: Y
            }), this.container),
            this.loadingSpan = $("span", null, d.labelStyle, c);
            this.loadingSpan.innerHTML = a || b.lang.loading;
            if (!this.loadingShown) E(c, {
                opacity: 0,
                display: "",
                left: this.plotLeft + "px",
                top: this.plotTop + "px",
                width: this.plotWidth + "px",
                height: this.plotHeight + "px"
            }),
            ob(c, {
                opacity: d.style.opacity
            },
            {
                duration: d.showDuration || 0
            }),
            this.loadingShown = !0
        },
        hideLoading: function() {
            var a = this.options,
            b = this.loadingDiv;
            b && ob(b, {
                opacity: 0
            },
            {
                duration: a.loading.hideDuration || 100,
                complete: function() {
                    E(b, {
                        display: Y
                    })
                }
            });
            this.loadingShown = !1
        }
    });
    v(za.prototype, {
        update: function(a, b, c) {
            var d = this,
            e = d.series,
            f = d.graphic,
            g, h = e.data,
            i = e.chart,
            k = e.options,
            b = o(b, !0);
            d.firePointEvent("update", {
                options: a
            },
            function() {
                d.applyOptions(a);
                if (fa(a)) {
                    e.getAttribs();
                    if (f) a && a.marker && a.marker.symbol ? d.graphic = f.destroy() : f.attr(d.pointAttr[d.state || ""]);
                    if (a && a.dataLabels && d.dataLabel) d.dataLabel = d.dataLabel.destroy()
                }
                g = wa(d, h);
                e.updateParallelArrays(d, g);
                k.data[g] = d.options;
                e.isDirty = e.isDirtyData = !0;
                if (!e.fixedBox && e.hasCartesianSeries) i.isDirtyBox = !0;
                k.legendType === "point" && i.legend.destroyItem(d);
                b && i.redraw(c)
            })
        },
        remove: function(a, b) {
            var c = this,
            d = c.series,
            e = d.points,
            f = d.chart,
            g, h = d.data;
            Ya(b, f);
            a = o(a, !0);
            c.firePointEvent("remove", null,
            function() {
                g = wa(c, h);
                h.length === e.length && e.splice(g, 1);
                h.splice(g, 1);
                d.options.data.splice(g, 1);
                d.updateParallelArrays(c, "splice", g, 1);
                c.destroy();
                d.isDirty = !0;
                d.isDirtyData = !0;
                a && f.redraw()
            })
        }
    });
    v(K.prototype, {
        addPoint: function(a, b, c, d) {
            var e = this.options,
            f = this.data,
            g = this.graph,
            h = this.area,
            i = this.chart,
            k = this.xAxis && this.xAxis.names,
            j = g && g.shift || 0,
            l = e.data,
            m, n = this.xData;
            Ya(d, i);
            c && q([g, h, this.graphNeg, this.areaNeg],
            function(a) {
                if (a) a.shift = j + 1
            });
            if (h) h.isArea = !0;
            b = o(b, !0);
            d = {
                series: this
            };
            this.pointClass.prototype.applyOptions.apply(d, [a]);
            g = d.x;
            h = n.length;
            if (this.requireSorting && g < n[h - 1]) for (m = !0; h && n[h - 1] > g;) h--;
            this.updateParallelArrays(d, "splice", h, 0, 0);
            this.updateParallelArrays(d, h);
            if (k) k[g] = d.name;
            l.splice(h, 0, a);
            m && (this.data.splice(h, 0, null), this.processData());
            e.legendType === "point" && this.generatePoints();
            c && (f[0] && f[0].remove ? f[0].remove(!1) : (f.shift(), this.updateParallelArrays(d, "shift"), l.shift()));
            this.isDirtyData = this.isDirty = !0;
            b && (this.getAttribs(), i.redraw())
        },
        remove: function(a, b) {
            var c = this,
            d = c.chart,
            a = o(a, !0);
            if (!c.isRemoving) c.isRemoving = !0,
            N(c, "remove", null,
            function() {
                c.destroy();
                d.isDirtyLegend = d.isDirtyBox = !0;
                d.linkSeries();
                a && d.redraw(b)
            });
            c.isRemoving = !1
        },
        update: function(a, b) {
            var c = this.chart,
            d = this.type,
            e = C[d].prototype,
            f,
            a = y(this.userOptions, {
                animation: !1,
                index: this.index,
                pointStart: this.xData[0]
            },
            {
                data: this.options.data
            },
            a);
            this.remove(!1);
            for (f in e) e.hasOwnProperty(f) && (this[f] = r);
            v(this, C[a.type || d].prototype);
            this.init(c, a);
            o(b, !0) && c.redraw(!1)
        }
    });
    v(L.prototype, {
        update: function(a, b) {
            var c = this.chart,
            a = c.options[this.coll][this.options.index] = y(this.userOptions, a);
            this.destroy(!0);
            this._addedPlotLB = r;
            this.init(c, v(a, {
                events: r
            }));
            c.isDirtyBox = !0;
            o(b, !0) && c.redraw()
        },
        remove: function(a) {
            for (var b = this.chart,
            c = this.coll,
            d = this.series,
            e = d.length; e--;) d[e] && d[e].remove(!1);
            pa(b.axes, this);
            pa(b[c], this);
            b.options[c].splice(this.options.index, 1);
            q(b[c],
            function(a, b) {
                a.options.index = b
            });
            this.destroy();
            b.isDirtyBox = !0;
            o(a, !0) && b.redraw()
        },
        setTitle: function(a, b) {
            this.update({
                title: a
            },
            b)
        },
        setCategories: function(a, b) {
            this.update({
                categories: a
            },
            b)
        }
    });
    var Aa = ga(K);
    C.line = Aa;
    T.area = y(J, {
        threshold: 0
    });
    var ta = ga(K, {
        type: "area",
        getSegments: function() {
            var a = [],
            b = [],
            c = [],
            d = this.xAxis,
            e = this.yAxis,
            f = e.stacks[this.stackKey],
            g = {},
            h,
            i,
            k = this.points,
            j = this.options.connectNulls,
            l,
            m,
            n;
            if (this.options.stacking && !this.cropped) {
                for (m = 0; m < k.length; m++) g[k[m].x] = k[m];
                for (n in f) f[n].total !== null && c.push( + n);
                c.sort(function(a, b) {
                    return a - b
                });
                q(c,
                function(a) {
                    if (!j || g[a] && g[a].y !== null) g[a] ? b.push(g[a]) : (h = d.translate(a), l = f[a].percent ? f[a].total ? f[a].cum * 100 / f[a].total: 0 : f[a].cum, i = e.toPixels(l, !0), b.push({
                        y: null,
                        plotX: h,
                        clientX: h,
                        plotY: i,
                        yBottom: i,
                        onMouseOver: na
                    }))
                });
                b.length && a.push(b)
            } else K.prototype.getSegments.call(this),
            a = this.segments;
            this.segments = a
        },
        getSegmentPath: function(a) {
            var b = K.prototype.getSegmentPath.call(this, a),
            c = [].concat(b),
            d,
            e = this.options;
            d = b.length;
            var f = this.yAxis.getThreshold(e.threshold),
            g;
            d === 3 && c.push("L", b[1], b[2]);
            if (e.stacking && !this.closedStacks) for (d = a.length - 1; d >= 0; d--) g = o(a[d].yBottom, f),
            d < a.length - 1 && e.step && c.push(a[d + 1].plotX, g),
            c.push(a[d].plotX, g);
            else this.closeSegment(c, a, f);
            this.areaPath = this.areaPath.concat(c);
            return b
        },
        closeSegment: function(a, b, c) {
            a.push("L", b[b.length - 1].plotX, c, "L", b[0].plotX, c)
        },
        drawGraph: function() {
            this.areaPath = [];
            K.prototype.drawGraph.apply(this);
            var a = this,
            b = this.areaPath,
            c = this.options,
            d = c.negativeColor,
            e = c.negativeFillColor,
            f = [["area", this.color, c.fillColor]]; (d || e) && f.push(["areaNeg", d, e]);
            q(f,
            function(d) {
                var e = d[0],
                f = a[e];
                f ? f.animate({
                    d: b
                }) : a[e] = a.chart.renderer.path(b).attr({
                    fill: o(d[2], Fa(d[1]).setOpacity(o(c.fillOpacity, 0.75)).get()),
                    zIndex: 0
                }).add(a.group)
            })
        },
        drawLegendSymbol: Q.drawRectangle
    });
    C.area = ta;
    T.spline = y(J);
    Aa = ga(K, {
        type: "spline",
        getPointSpline: function(a, b, c) {
            var d = b.plotX,
            e = b.plotY,
            f = a[c - 1],
            g = a[c + 1],
            h,
            i,
            k,
            j;
            if (f && g) {
                a = f.plotY;
                k = g.plotX;
                var g = g.plotY,
                l;
                h = (1.5 * d + f.plotX) / 2.5;
                i = (1.5 * e + a) / 2.5;
                k = (1.5 * d + k) / 2.5;
                j = (1.5 * e + g) / 2.5;
                l = (j - i) * (k - d) / (k - h) + e - j;
                i += l;
                j += l;
                i > a && i > e ? (i = w(a, e), j = 2 * e - i) : i < a && i < e && (i = z(a, e), j = 2 * e - i);
                j > g && j > e ? (j = w(g, e), i = 2 * e - j) : j < g && j < e && (j = z(g, e), i = 2 * e - j);
                b.rightContX = k;
                b.rightContY = j
            }
            c ? (b = ["C", f.rightContX || f.plotX, f.rightContY || f.plotY, h || d, i || e, d, e], f.rightContX = f.rightContY = null) : b = ["M", d, e];
            return b
        }
    });
    C.spline = Aa;
    T.areaspline = y(T.area);
    ta = ta.prototype;
    Aa = ga(Aa, {
        type: "areaspline",
        closedStacks: !0,
        getSegmentPath: ta.getSegmentPath,
        closeSegment: ta.closeSegment,
        drawGraph: ta.drawGraph,
        drawLegendSymbol: Q.drawRectangle
    });
    C.areaspline = Aa;
    T.column = y(J, {
        borderColor: "#FFFFFF",
        borderRadius: 0,
        groupPadding: 0.2,
        marker: null,
        pointPadding: 0.1,
        minPointLength: 0,
        cropThreshold: 50,
        pointRange: null,
        states: {
            hover: {
                brightness: 0.1,
                shadow: !1,
                halo: !1
            },
            select: {
                color: "#C0C0C0",
                borderColor: "#000000",
                shadow: !1
            }
        },
        dataLabels: {
            align: null,
            verticalAlign: null,
            y: null
        },
        stickyTracking: !1,
        tooltip: {
            distance: 6
        },
        threshold: 0
    });
    Aa = ga(K, {
        type: "column",
        pointAttrToOptions: {
            stroke: "borderColor",
            fill: "color",
            r: "borderRadius"
        },
        cropShoulder: 0,
        trackerGroups: ["group", "dataLabelsGroup"],
        negStacks: !0,
        init: function() {
            K.prototype.init.apply(this, arguments);
            var a = this,
            b = a.chart;
            b.hasRendered && q(b.series,
            function(b) {
                if (b.type === a.type) b.isDirty = !0
            })
        },
        getColumnMetrics: function() {
            var a = this,
            b = a.options,
            c = a.xAxis,
            d = a.yAxis,
            e = c.reversed,
            f, g = {},
            h, i = 0;
            b.grouping === !1 ? i = 1 : q(a.chart.series,
            function(b) {
                var c = b.options,
                e = b.yAxis;
                if (b.type === a.type && b.visible && d.len === e.len && d.pos === e.pos) c.stacking ? (f = b.stackKey, g[f] === r && (g[f] = i++), h = g[f]) : c.grouping !== !1 && (h = i++),
                b.columnIndex = h
            });
            var c = z(M(c.transA) * (c.ordinalSlope || b.pointRange || c.closestPointRange || c.tickInterval || 1), c.len),
            k = c * b.groupPadding,
            j = (c - 2 * k) / i,
            l = b.pointWidth,
            b = t(l) ? (j - l) / 2 : j * b.pointPadding,
            l = o(l, j - 2 * b);
            return a.columnMetrics = {
                width: l,
                offset: b + (k + ((e ? i - (a.columnIndex || 0) : a.columnIndex) || 0) * j - c / 2) * (e ? -1 : 1)
            }
        },
        translate: function() {
            var a = this,
            b = a.chart,
            c = a.options,
            d = a.borderWidth = o(c.borderWidth, a.activePointCount > 0.5 * a.xAxis.len ? 0 : 1),
            e = a.yAxis,
            f = a.translatedThreshold = e.getThreshold(c.threshold),
            g = o(c.minPointLength, 5),
            c = a.getColumnMetrics(),
            h = c.width,
            i = a.barW = Va(w(h, 1 + 2 * d)),
            k = a.pointXOffset = c.offset,
            j = -(d % 2 ? 0.5 : 0),
            l = d % 2 ? 0.5 : 1;
            b.renderer.isVML && b.inverted && (l += 1);
            K.prototype.translate.apply(a);
            q(a.points,
            function(c) {
                var d = o(c.yBottom, f),
                p = z(w( - 999 - d, c.plotY), e.len + 999 + d),
                u = c.plotX + k,
                q = i,
                r = z(p, d),
                x;
                x = w(p, d) - r;
                M(x) < g && g && (x = g, r = s(M(r - f) > g ? d - g: f - (e.translate(c.y, 0, 1, 0, 1) <= f ? g: 0)));
                c.barX = u;
                c.pointWidth = h;
                c.tooltipPos = b.inverted ? [e.len - p, a.xAxis.len - u - q / 2] : [u + q / 2, p];
                d = M(u) < 0.5;
                q = s(u + q) + j;
                u = s(u) + j;
                q -= u;
                p = M(r) < 0.5;
                x = s(r + x) + l;
                r = s(r) + l;
                x -= r;
                d && (u += 1, q -= 1);
                p && (r -= 1, x += 1);
                c.shapeType = "rect";
                c.shapeArgs = {
                    x: u,
                    y: r,
                    width: q,
                    height: x
                }
            })
        },
        getSymbol: na,
        drawLegendSymbol: Q.drawRectangle,
        drawGraph: na,
        drawPoints: function() {
            var a = this,
            b = this.chart,
            c = a.options,
            d = b.renderer,
            e = c.animationLimit || 250,
            f, g, h;
            q(a.points,
            function(i) {
                var k = i.plotY,
                j = i.graphic;
                if (k !== r && !isNaN(k) && i.y !== null) f = i.shapeArgs,
                h = t(a.borderWidth) ? {
                    "stroke-width": a.borderWidth
                }: {},
                g = i.pointAttr[i.selected ? "select": ""] || a.pointAttr[""],
                j ? (eb(j), j.attr(h)[b.pointCount < e ? "animate": "attr"](y(f))) : i.graphic = d[i.shapeType](f).attr(g).attr(h).add(a.group).shadow(c.shadow, null, c.stacking && !c.borderRadius);
                else if (j) i.graphic = j.destroy()
            })
        },
        animate: function(a) {
            var b = this.yAxis,
            c = this.options,
            d = this.chart.inverted,
            e = {};
            if (ca) a ? (e.scaleY = 0.001, a = z(b.pos + b.len, w(b.pos, b.toPixels(c.threshold))), d ? e.translateX = a - b.len: e.translateY = a, this.group.attr(e)) : (e.scaleY = 1, e[d ? "translateX": "translateY"] = b.pos, this.group.animate(e, this.options.animation), this.animate = null)
        },
        remove: function() {
            var a = this,
            b = a.chart;
            b.hasRendered && q(b.series,
            function(b) {
                if (b.type === a.type) b.isDirty = !0
            });
            K.prototype.remove.apply(a, arguments)
        }
    });
    C.column = Aa;
    T.bar = y(T.column);
    ta = ga(Aa, {
        type: "bar",
        inverted: !0
    });
    C.bar = ta;
    T.scatter = y(J, {
        lineWidth: 0,
        tooltip: {
            headerFormat: '<span style="color:{series.color}">●</span> <span style="font-size: 10px;"> {series.name}</span><br/>',
            pointFormat: "x: <b>{point.x}</b><br/>y: <b>{point.y}</b><br/>"
        },
        stickyTracking: !1
    });
    ta = ga(K, {
        type: "scatter",
        sorted: !1,
        requireSorting: !1,
        noSharedTooltip: !0,
        trackerGroups: ["markerGroup"],
        takeOrdinalPosition: !1,
        singularTooltips: !0,
        drawGraph: function() {
            this.options.lineWidth && K.prototype.drawGraph.call(this)
        }
    });
    C.scatter = ta;
    T.pie = y(J, {
        borderColor: "#FFFFFF",
        borderWidth: 1,
        center: [null, null],
        clip: !1,
        colorByPoint: !0,
        dataLabels: {
            distance: 30,
            enabled: !0,
            formatter: function() {
                return this.point.name
            }
        },
        ignoreHiddenPoint: !0,
        legendType: "point",
        marker: null,
        size: null,
        showInLegend: !1,
        slicedOffset: 10,
        states: {
            hover: {
                brightness: 0.1,
                shadow: !1
            }
        },
        stickyTracking: !1,
        tooltip: {
            followPointer: !0
        }
    });
    J = {
        type: "pie",
        isCartesian: !1,
        pointClass: ga(za, {
            init: function() {
                za.prototype.init.apply(this, arguments);
                var a = this,
                b;
                if (a.y < 0) a.y = null;
                v(a, {
                    visible: a.visible !== !1,
                    name: o(a.name, "Slice")
                });
                b = function(b) {
                    a.slice(b.type === "select")
                };
                A(a, "select", b);
                A(a, "unselect", b);
                return a
            },
            setVisible: function(a) {
                var b = this,
                c = b.series,
                d = c.chart;
                b.visible = b.options.visible = a = a === r ? !b.visible: a;
                c.options.data[wa(b, c.data)] = b.options;
                q(["graphic", "dataLabel", "connector", "shadowGroup"],
                function(c) {
                    if (b[c]) b[c][a ? "show": "hide"](!0)
                });
                b.legendItem && d.legend.colorizeItem(b, a);
                if (!c.isDirty && c.options.ignoreHiddenPoint) c.isDirty = !0,
                d.redraw()
            },
            slice: function(a, b, c) {
                var d = this.series;
                Ya(c, d.chart);
                o(b, !0);
                this.sliced = this.options.sliced = a = t(a) ? a: !this.sliced;
                d.options.data[wa(this, d.data)] = this.options;
                a = a ? this.slicedTranslation: {
                    translateX: 0,
                    translateY: 0
                };
                this.graphic.animate(a);
                this.shadowGroup && this.shadowGroup.animate(a)
            },
            haloPath: function(a) {
                var b = this.shapeArgs,
                c = this.series.chart;
                return this.series.chart.renderer.symbols.arc(c.plotLeft + b.x, c.plotTop + b.y, b.r + a, b.r + a, {
                    innerR: this.shapeArgs.r,
                    start: b.start,
                    end: b.end
                })
            }
        }),
        requireSorting: !1,
        noSharedTooltip: !0,
        trackerGroups: ["group", "dataLabelsGroup"],
        axisTypes: [],
        pointAttrToOptions: {
            stroke: "borderColor",
            "stroke-width": "borderWidth",
            fill: "color"
        },
        singularTooltips: !0,
        getColor: na,
        animate: function(a) {
            var b = this,
            c = b.points,
            d = b.startAngleRad;
            if (!a) q(c,
            function(a) {
                var c = a.graphic,
                a = a.shapeArgs;
                c && (c.attr({
                    r: b.center[3] / 2,
                    start: d,
                    end: d
                }), c.animate({
                    r: a.r,
                    start: a.start,
                    end: a.end
                },
                b.options.animation))
            }),
            b.animate = null
        },
        setData: function(a, b, c, d) {
            K.prototype.setData.call(this, a, !1, c, d);
            this.processData();
            this.generatePoints();
            o(b, !0) && this.chart.redraw(c)
        },
        generatePoints: function() {
            var a, b = 0,
            c, d, e, f = this.options.ignoreHiddenPoint;
            K.prototype.generatePoints.call(this);
            c = this.points;
            d = c.length;
            for (a = 0; a < d; a++) e = c[a],
            b += f && !e.visible ? 0 : e.y;
            this.total = b;
            for (a = 0; a < d; a++) e = c[a],
            e.percentage = b > 0 ? e.y / b * 100 : 0,
            e.total = b
        },
        translate: function(a) {
            this.generatePoints();
            var b = 0,
            c = this.options,
            d = c.slicedOffset,
            e = d + c.borderWidth,
            f, g, h, i = c.startAngle || 0,
            k = this.startAngleRad = ra / 180 * (i - 90),
            i = (this.endAngleRad = ra / 180 * (o(c.endAngle, i + 360) - 90)) - k,
            j = this.points,
            l = c.dataLabels.distance,
            c = c.ignoreHiddenPoint,
            m,
            n = j.length,
            p;
            if (!a) this.center = a = this.getCenter();
            this.getX = function(b, c) {
                h = V.asin(z((b - a[1]) / (a[2] / 2 + l), 1));
                return a[0] + (c ? -1 : 1) * da(h) * (a[2] / 2 + l)
            };
            for (m = 0; m < n; m++) {
                p = j[m];
                f = k + b * i;
                if (!c || p.visible) b += p.percentage / 100;
                g = k + b * i;
                p.shapeType = "arc";
                p.shapeArgs = {
                    x: a[0],
                    y: a[1],
                    r: a[2] / 2,
                    innerR: a[3] / 2,
                    start: s(f * 1E3) / 1E3,
                    end: s(g * 1E3) / 1E3
                };
                h = (g + f) / 2;
                h > 1.5 * ra ? h -= 2 * ra: h < -ra / 2 && (h += 2 * ra);
                p.slicedTranslation = {
                    translateX: s(da(h) * d),
                    translateY: s(ia(h) * d)
                };
                f = da(h) * a[2] / 2;
                g = ia(h) * a[2] / 2;
                p.tooltipPos = [a[0] + f * 0.7, a[1] + g * 0.7];
                p.half = h < -ra / 2 || h > ra / 2 ? 1 : 0;
                p.angle = h;
                e = z(e, l / 2);
                p.labelPos = [a[0] + f + da(h) * l, a[1] + g + ia(h) * l, a[0] + f + da(h) * e, a[1] + g + ia(h) * e, a[0] + f, a[1] + g, l < 0 ? "center": p.half ? "right": "left", h]
            }
        },
        drawGraph: null,
        drawPoints: function() {
            var a = this,
            b = a.chart.renderer,
            c, d, e = a.options.shadow,
            f, g;
            if (e && !a.shadowGroup) a.shadowGroup = b.g("shadow").add(a.group);
            q(a.points,
            function(h) {
                d = h.graphic;
                g = h.shapeArgs;
                f = h.shadowGroup;
                if (e && !f) f = h.shadowGroup = b.g("shadow").add(a.shadowGroup);
                c = h.sliced ? h.slicedTranslation: {
                    translateX: 0,
                    translateY: 0
                };
                f && f.attr(c);
                d ? d.animate(v(g, c)) : h.graphic = d = b[h.shapeType](g).setRadialReference(a.center).attr(h.pointAttr[h.selected ? "select": ""]).attr({
                    "stroke-linejoin": "round"
                }).attr(c).add(a.group).shadow(e, f);
                h.visible !== void 0 && h.setVisible(h.visible)
            })
        },
        sortByAngle: function(a, b) {
            a.sort(function(a, d) {
                return a.angle !== void 0 && (d.angle - a.angle) * b
            })
        },
        drawLegendSymbol: Q.drawRectangle,
        getCenter: ea.getCenter,
        getSymbol: na
    };
    J = ga(K, J);
    C.pie = J;
    K.prototype.drawDataLabels = function() {
        var a = this,
        b = a.options,
        c = b.cursor,
        d = b.dataLabels,
        e = a.points,
        f, g, h, i;
        if (d.enabled || a._hasPointLabels) a.dlProcessOptions && a.dlProcessOptions(d),
        i = a.plotGroup("dataLabelsGroup", "data-labels", "hidden", d.zIndex || 6),
        !a.hasRendered && o(d.defer, !0) && (i.attr({
            opacity: 0
        }), A(a, "afterAnimate",
        function() {
            a.dataLabelsGroup.show()[b.animation ? "animate": "attr"]({
                opacity: 1
            },
            {
                duration: 200
            })
        })),
        g = d,
        q(e,
        function(b) {
            var e, l = b.dataLabel,
            m, n, p = b.connector,
            u = !0;
            f = b.options && b.options.dataLabels;
            e = o(f && f.enabled, g.enabled);
            if (l && !e) b.dataLabel = l.destroy();
            else if (e) {
                d = y(g, f);
                e = d.rotation;
                m = b.getLabelConfig();
                h = d.format ? Ja(d.format, m) : d.formatter.call(m, d);
                d.style.color = o(d.color, d.style.color, a.color, "black");
                if (l) if (t(h)) l.attr({
                    text: h
                }),
                u = !1;
                else {
                    if (b.dataLabel = l = l.destroy(), p) b.connector = p.destroy()
                } else if (t(h)) {
                    l = {
                        fill: d.backgroundColor,
                        stroke: d.borderColor,
                        "stroke-width": d.borderWidth,
                        r: d.borderRadius || 0,
                        rotation: e,
                        padding: d.padding,
                        zIndex: 1
                    };
                    for (n in l) l[n] === r && delete l[n];
                    l = b.dataLabel = a.chart.renderer[e ? "text": "label"](h, 0, -999, null, null, null, d.useHTML).attr(l).css(v(d.style, c && {
                        cursor: c
                    })).add(i).shadow(d.shadow)
                }
                l && a.alignDataLabel(b, l, d, null, u)
            }
        })
    };
    K.prototype.alignDataLabel = function(a, b, c, d, e) {
        var f = this.chart,
        g = f.inverted,
        h = o(a.plotX, -999),
        i = o(a.plotY, -999),
        k = b.getBBox();
        if (a = this.visible && (a.series.forceDL || f.isInsidePlot(h, s(i), g) || d && f.isInsidePlot(h, g ? d.x + 1 : d.y + d.height - 1, g))) d = v({
            x: g ? f.plotWidth - i: h,
            y: s(g ? f.plotHeight - h: i),
            width: 0,
            height: 0
        },
        d),
        v(c, {
            width: k.width,
            height: k.height
        }),
        c.rotation ? (g = {
            align: c.align,
            x: d.x + c.x + d.width / 2,
            y: d.y + c.y + d.height / 2
        },
        b[e ? "attr": "animate"](g)) : (b.align(c, null, d), g = b.alignAttr, o(c.overflow, "justify") === "justify" ? this.justifyDataLabel(b, c, g, k, d, e) : o(c.crop, !0) && (a = f.isInsidePlot(g.x, g.y) && f.isInsidePlot(g.x + k.width, g.y + k.height)));
        if (!a) b.attr({
            y: -999
        }),
        b.placed = !1
    };
    K.prototype.justifyDataLabel = function(a, b, c, d, e, f) {
        var g = this.chart,
        h = b.align,
        i = b.verticalAlign,
        k, j;
        k = c.x;
        if (k < 0) h === "right" ? b.align = "left": b.x = -k,
        j = !0;
        k = c.x + d.width;
        if (k > g.plotWidth) h === "left" ? b.align = "right": b.x = g.plotWidth - k,
        j = !0;
        k = c.y;
        if (k < 0) i === "bottom" ? b.verticalAlign = "top": b.y = -k,
        j = !0;
        k = c.y + d.height;
        if (k > g.plotHeight) i === "top" ? b.verticalAlign = "bottom": b.y = g.plotHeight - k,
        j = !0;
        if (j) a.placed = !f,
        a.align(b, null, e)
    };
    if (C.pie) C.pie.prototype.drawDataLabels = function() {
        var a = this,
        b = a.data,
        c, d = a.chart,
        e = a.options.dataLabels,
        f = o(e.connectorPadding, 10),
        g = o(e.connectorWidth, 1),
        h = d.plotWidth,
        d = d.plotHeight,
        i,
        k,
        j = o(e.softConnector, !0),
        l = e.distance,
        m = a.center,
        n = m[2] / 2,
        p = m[1],
        u = l > 0,
        r,
        G,
        x,
        t,
        v = [[], []],
        y,
        z,
        A,
        B,
        D,
        U = [0, 0, 0, 0],
        I = function(a, b) {
            return b.y - a.y
        };
        if (a.visible && (e.enabled || a._hasPointLabels)) {
            K.prototype.drawDataLabels.apply(a);
            q(b,
            function(a) {
                a.dataLabel && a.visible && v[a.half].push(a)
            });
            for (B = 0; ! t && b[B];) t = b[B] && b[B].dataLabel && (b[B].dataLabel.getBBox().height || 21),
            B++;
            for (B = 2; B--;) {
                var b = [],
                H = [],
                C = v[B],
                E = C.length,
                F;
                a.sortByAngle(C, B - 0.5);
                if (l > 0) {
                    for (D = p - n - l; D <= p + n + l; D += t) b.push(D);
                    G = b.length;
                    if (E > G) {
                        c = [].concat(C);
                        c.sort(I);
                        for (D = E; D--;) c[D].rank = D;
                        for (D = E; D--;) C[D].rank >= G && C.splice(D, 1);
                        E = C.length
                    }
                    for (D = 0; D < E; D++) {
                        c = C[D];
                        x = c.labelPos;
                        c = 9999;
                        var L, J;
                        for (J = 0; J < G; J++) L = M(b[J] - x[1]),
                        L < c && (c = L, F = J);
                        if (F < D && b[D] !== null) F = D;
                        else for (G < E - D + F && b[D] !== null && (F = G - E + D); b[F] === null;) F++;
                        H.push({
                            i: F,
                            y: b[F]
                        });
                        b[F] = null
                    }
                    H.sort(I)
                }
                for (D = 0; D < E; D++) {
                    c = C[D];
                    x = c.labelPos;
                    r = c.dataLabel;
                    A = c.visible === !1 ? "hidden": "visible";
                    c = x[1];
                    if (l > 0) {
                        if (G = H.pop(), F = G.i, z = G.y, c > z && b[F + 1] !== null || c < z && b[F - 1] !== null) z = c
                    } else z = c;
                    y = e.justify ? m[0] + (B ? -1 : 1) * (n + l) : a.getX(F === 0 || F === b.length - 1 ? c: z, B);
                    r._attr = {
                        visibility: A,
                        align: x[6]
                    };
                    r._pos = {
                        x: y + e.x + ({
                            left: f,
                            right: -f
                        } [x[6]] || 0),
                        y: z + e.y - 10
                    };
                    r.connX = y;
                    r.connY = z;
                    if (this.options.size === null) G = r.width,
                    y - G < f ? U[3] = w(s(G - y + f), U[3]) : y + G > h - f && (U[1] = w(s(y + G - h + f), U[1])),
                    z - t / 2 < 0 ? U[0] = w(s( - z + t / 2), U[0]) : z + t / 2 > d && (U[2] = w(s(z + t / 2 - d), U[2]))
                }
            }
            if (Ba(U) === 0 || this.verifyDataLabelOverflow(U)) this.placeDataLabels(),
            u && g && q(this.points,
            function(b) {
                i = b.connector;
                x = b.labelPos;
                if ((r = b.dataLabel) && r._pos) A = r._attr.visibility,
                y = r.connX,
                z = r.connY,
                k = j ? ["M", y + (x[6] === "left" ? 5 : -5), z, "C", y, z, 2 * x[2] - x[4], 2 * x[3] - x[5], x[2], x[3], "L", x[4], x[5]] : ["M", y + (x[6] === "left" ? 5 : -5), z, "L", x[2], x[3], "L", x[4], x[5]],
                i ? (i.animate({
                    d: k
                }), i.attr("visibility", A)) : b.connector = i = a.chart.renderer.path(k).attr({
                    "stroke-width": g,
                    stroke: e.connectorColor || b.color || "#606060",
                    visibility: A
                }).add(a.dataLabelsGroup);
                else if (i) b.connector = i.destroy()
            })
        }
    },
    C.pie.prototype.placeDataLabels = function() {
        q(this.points,
        function(a) {
            var a = a.dataLabel,
            b;
            if (a)(b = a._pos) ? (a.attr(a._attr), a[a.moved ? "animate": "attr"](b), a.moved = !0) : a && a.attr({
                y: -999
            })
        })
    },
    C.pie.prototype.alignDataLabel = na,
    C.pie.prototype.verifyDataLabelOverflow = function(a) {
        var b = this.center,
        c = this.options,
        d = c.center,
        e = c = c.minSize || 80,
        f;
        d[0] !== null ? e = w(b[2] - w(a[1], a[3]), c) : (e = w(b[2] - a[1] - a[3], c), b[0] += (a[3] - a[1]) / 2);
        d[1] !== null ? e = w(z(e, b[2] - w(a[0], a[2])), c) : (e = w(z(e, b[2] - a[0] - a[2]), c), b[1] += (a[0] - a[2]) / 2);
        e < b[2] ? (b[2] = e, this.translate(b), q(this.points,
        function(a) {
            if (a.dataLabel) a.dataLabel._pos = null
        }), this.drawDataLabels && this.drawDataLabels()) : f = !0;
        return f
    };
    if (C.column) C.column.prototype.alignDataLabel = function(a, b, c, d, e) {
        var f = this.chart,
        g = f.inverted,
        h = a.dlBox || a.shapeArgs,
        i = a.below || a.plotY > o(this.translatedThreshold, f.plotSizeY),
        k = o(c.inside, !!this.options.stacking);
        if (h && (d = y(h), g && (d = {
            x: f.plotWidth - d.y - d.height,
            y: f.plotHeight - d.x - d.width,
            width: d.height,
            height: d.width
        }), !k)) g ? (d.x += i ? 0 : d.width, d.width = 0) : (d.y += i ? d.height: 0, d.height = 0);
        c.align = o(c.align, !g || k ? "center": i ? "right": "left");
        c.verticalAlign = o(c.verticalAlign, g || k ? "middle": i ? "top": "bottom");
        K.prototype.alignDataLabel.call(this, a, b, c, d, e)
    };
    var gb = P.TrackerMixin = {
        drawTrackerPoint: function() {
            var a = this,
            b = a.chart,
            c = b.pointer,
            d = a.options.cursor,
            e = d && {
                cursor: d
            },
            f = function(c) {
                var d = c.target,
                e;
                if (b.hoverSeries !== a) a.onMouseOver();
                for (; d && !e;) e = d.point,
                d = d.parentNode;
                if (e !== r && e !== b.hoverPoint) e.onMouseOver(c)
            };
            q(a.points,
            function(a) {
                if (a.graphic) a.graphic.element.point = a;
                if (a.dataLabel) a.dataLabel.element.point = a
            });
            if (!a._hasTracking) q(a.trackerGroups,
            function(b) {
                if (a[b] && (a[b].addClass("highcharts-tracker").on("mouseover", f).on("mouseout",
                function(a) {
                    c.onTrackerMouseOut(a)
                }).css(e), ab)) a[b].on("touchstart", f)
            }),
            a._hasTracking = !0
        },
        drawTrackerGraph: function() {
            var a = this,
            b = a.options,
            c = b.trackByArea,
            d = [].concat(c ? a.areaPath: a.graphPath),
            e = d.length,
            f = a.chart,
            g = f.pointer,
            h = f.renderer,
            i = f.options.tooltip.snap,
            k = a.tracker,
            j = b.cursor,
            l = j && {
                cursor: j
            },
            j = a.singlePoints,
            m,
            n = function() {
                if (f.hoverSeries !== a) a.onMouseOver()
            },
            p = "rgba(192,192,192," + (ca ? 1.0E-4: 0.002) + ")";
            if (e && !c) for (m = e + 1; m--;) d[m] === "M" && d.splice(m + 1, 0, d[m + 1] - i, d[m + 2], "L"),
            (m && d[m] === "M" || m === e) && d.splice(m, 0, "L", d[m - 2] + i, d[m - 1]);
            for (m = 0; m < j.length; m++) e = j[m],
            d.push("M", e.plotX - i, e.plotY, "L", e.plotX + i, e.plotY);
            k ? k.attr({
                d: d
            }) : (a.tracker = h.path(d).attr({
                "stroke-linejoin": "round",
                visibility: a.visible ? "visible": "hidden",
                stroke: p,
                fill: c ? p: Y,
                "stroke-width": b.lineWidth + (c ? 0 : 2 * i),
                zIndex: 2
            }).add(a.group), q([a.tracker, a.markerGroup],
            function(a) {
                a.addClass("highcharts-tracker").on("mouseover", n).on("mouseout",
                function(a) {
                    g.onTrackerMouseOut(a)
                }).css(l);
                if (ab) a.on("touchstart", n)
            }))
        }
    };
    if (C.column) Aa.prototype.drawTracker = gb.drawTrackerPoint;
    if (C.pie) C.pie.prototype.drawTracker = gb.drawTrackerPoint;
    if (C.scatter) ta.prototype.drawTracker = gb.drawTrackerPoint;
    v(pb.prototype, {
        setItemEvents: function(a, b, c, d, e) {
            var f = this; (c ? b: a.legendGroup).on("mouseover",
            function() {
                a.setState("hover");
                b.css(f.options.itemHoverStyle)
            }).on("mouseout",
            function() {
                b.css(a.visible ? d: e);
                a.setState()
            }).on("click",
            function(b) {
                var c = function() {
                    a.setVisible()
                },
                b = {
                    browserEvent: b
                };
                a.firePointEvent ? a.firePointEvent("legendItemClick", b, c) : N(a, "legendItemClick", b, c)
            })
        },
        createCheckboxForItem: function(a) {
            a.checkbox = $("input", {
                type: "checkbox",
                checked: a.selected,
                defaultChecked: a.selected
            },
            this.options.itemCheckboxStyle, this.chart.container);
            A(a.checkbox, "click",
            function(b) {
                N(a, "checkboxClick", {
                    checked: b.target.checked
                },
                function() {
                    a.select()
                })
            })
        }
    });
    F.legend.itemStyle.cursor = "pointer";
    v(va.prototype, {
        showResetZoom: function() {
            var a = this,
            b = F.lang,
            c = a.options.chart.resetZoomButton,
            d = c.theme,
            e = d.states,
            f = c.relativeTo === "chart" ? null: "plotBox";
            this.resetZoomButton = a.renderer.button(b.resetZoom, null, null,
            function() {
                a.zoomOut()
            },
            d, e && e.hover).attr({
                align: c.position.align,
                title: b.resetZoomTitle
            }).add().align(c.position, !1, f)
        },
        zoomOut: function() {
            var a = this;
            N(a, "selection", {
                resetSelection: !0
            },
            function() {
                a.zoom()
            })
        },
        zoom: function(a) {
            var b, c = this.pointer,
            d = !1,
            e; ! a || a.resetSelection ? q(this.axes,
            function(a) {
                b = a.zoom()
            }) : q(a.xAxis.concat(a.yAxis),
            function(a) {
                var e = a.axis,
                h = e.isXAxis;
                if (c[h ? "zoomX": "zoomY"] || c[h ? "pinchX": "pinchY"]) b = e.zoom(a.min, a.max),
                e.displayBtn && (d = !0)
            });
            e = this.resetZoomButton;
            if (d && !e) this.showResetZoom();
            else if (!d && fa(e)) this.resetZoomButton = e.destroy();
            b && this.redraw(o(this.options.chart.animation, a && a.animation, this.pointCount < 100))
        },
        pan: function(a, b) {
            var c = this,
            d = c.hoverPoints,
            e;
            d && q(d,
            function(a) {
                a.setState()
            });
            q(b === "xy" ? [1, 0] : [1],
            function(b) {
                var d = a[b ? "chartX": "chartY"],
                h = c[b ? "xAxis": "yAxis"][0],
                i = c[b ? "mouseDownX": "mouseDownY"],
                k = (h.pointRange || 0) / 2,
                j = h.getExtremes(),
                l = h.toValue(i - d, !0) + k,
                i = h.toValue(i + c[b ? "plotWidth": "plotHeight"] - d, !0) - k;
                h.series.length && l > z(j.dataMin, j.min) && i < w(j.dataMax, j.max) && (h.setExtremes(l, i, !1, !1, {
                    trigger: "pan"
                }), e = !0);
                c[b ? "mouseDownX": "mouseDownY"] = d
            });
            e && c.redraw(!1);
            E(c.container, {
                cursor: "move"
            })
        }
    });
    v(za.prototype, {
        select: function(a, b) {
            var c = this,
            d = c.series,
            e = d.chart,
            a = o(a, !c.selected);
            c.firePointEvent(a ? "select": "unselect", {
                accumulate: b
            },
            function() {
                c.selected = c.options.selected = a;
                d.options.data[wa(c, d.data)] = c.options;
                c.setState(a && "select");
                b || q(e.getSelectedPoints(),
                function(a) {
                    if (a.selected && a !== c) a.selected = a.options.selected = !1,
                    d.options.data[wa(a, d.data)] = a.options,
                    a.setState(""),
                    a.firePointEvent("unselect")
                })
            })
        },
        onMouseOver: function(a) {
            var b = this.series,
            c = b.chart,
            d = c.tooltip,
            e = c.hoverPoint;
            if (e && e !== this) e.onMouseOut();
            this.firePointEvent("mouseOver");
            d && (!d.shared || b.noSharedTooltip) && d.refresh(this, a);
            this.setState("hover");
            c.hoverPoint = this
        },
        onMouseOut: function() {
            var a = this.series.chart,
            b = a.hoverPoints;
            if (!b || wa(this, b) === -1) this.firePointEvent("mouseOut"),
            this.setState(),
            a.hoverPoint = null
        },
        importEvents: function() {
            if (!this.hasImportedEvents) {
                var a = y(this.series.options.point, this.options).events,
                b;
                this.events = a;
                for (b in a) A(this, b, a[b]);
                this.hasImportedEvents = !0
            }
        },
        setState: function(a, b) {
            var c = this.plotX,
            d = this.plotY,
            e = this.series,
            f = e.options.states,
            g = T[e.type].marker && e.options.marker,
            h = g && !g.enabled,
            i = g && g.states[a],
            k = i && i.enabled === !1,
            j = e.stateMarkerGraphic,
            l = this.marker || {},
            m = e.chart,
            n = e.halo,
            p,
            a = a || "";
            p = this.pointAttr[a] || e.pointAttr[a];
            if (! (a === this.state && !b || this.selected && a !== "select" || f[a] && f[a].enabled === !1 || a && (k || h && i.enabled === !1) || a && l.states && l.states[a] && l.states[a].enabled === !1)) {
                if (this.graphic) g = g && this.graphic.symbolName && p.r,
                this.graphic.attr(y(p, g ? {
                    x: c - g,
                    y: d - g,
                    width: 2 * g,
                    height: 2 * g
                }: {})),
                j && j.hide();
                else {
                    if (a && i) if (g = i.radius, l = l.symbol || e.symbol, j && j.currentSymbol !== l && (j = j.destroy()), j) j[b ? "animate": "attr"]({
                        x: c - g,
                        y: d - g
                    });
                    else if (l) e.stateMarkerGraphic = j = m.renderer.symbol(l, c - g, d - g, 2 * g, 2 * g).attr(p).add(e.markerGroup),
                    j.currentSymbol = l;
                    if (j) j[a && m.isInsidePlot(c, d, m.inverted) ? "show": "hide"]()
                }
                if ((c = f[a] && f[a].halo) && c.size) {
                    if (!n) e.halo = n = m.renderer.path().add(e.seriesGroup);
                    n.attr(v({
                        fill: Fa(this.color || e.color).setOpacity(c.opacity).get()
                    },
                    c.attributes))[b ? "animate": "attr"]({
                        d: this.haloPath(c.size)
                    })
                } else n && n.attr({
                    d: []
                });
                this.state = a
            }
        },
        haloPath: function(a) {
            var b = this.series,
            c = b.chart,
            d = b.getPlotBox(),
            e = c.inverted;
            return c.renderer.symbols.circle(d.translateX + (e ? b.yAxis.len - this.plotY: this.plotX) - a, d.translateY + (e ? b.xAxis.len - this.plotX: this.plotY) - a, a * 2, a * 2)
        }
    });
    v(K.prototype, {
        onMouseOver: function() {
            var a = this.chart,
            b = a.hoverSeries;
            if (b && b !== this) b.onMouseOut();
            this.options.events.mouseOver && N(this, "mouseOver");
            this.setState("hover");
            a.hoverSeries = this
        },
        onMouseOut: function() {
            var a = this.options,
            b = this.chart,
            c = b.tooltip,
            d = b.hoverPoint;
            if (d) d.onMouseOut();
            this && a.events.mouseOut && N(this, "mouseOut");
            c && !a.stickyTracking && (!c.shared || this.noSharedTooltip) && c.hide();
            this.setState();
            b.hoverSeries = null
        },
        setState: function(a) {
            var b = this.options,
            c = this.graph,
            d = this.graphNeg,
            e = b.states,
            b = b.lineWidth,
            a = a || "";
            if (this.state !== a) this.state = a,
            e[a] && e[a].enabled === !1 || (a && (b = e[a].lineWidth || b + 1), c && !c.dashstyle && (a = {
                "stroke-width": b
            },
            c.attr(a), d && d.attr(a)))
        },
        setVisible: function(a, b) {
            var c = this,
            d = c.chart,
            e = c.legendItem,
            f, g = d.options.chart.ignoreHiddenSeries,
            h = c.visible;
            f = (c.visible = a = c.userOptions.visible = a === r ? !h: a) ? "show": "hide";
            q(["group", "dataLabelsGroup", "markerGroup", "tracker"],
            function(a) {
                if (c[a]) c[a][f]()
            });
            if (d.hoverSeries === c) c.onMouseOut();
            e && d.legend.colorizeItem(c, a);
            c.isDirty = !0;
            c.options.stacking && q(d.series,
            function(a) {
                if (a.options.stacking && a.visible) a.isDirty = !0
            });
            q(c.linkedSeries,
            function(b) {
                b.setVisible(a, !1)
            });
            if (g) d.isDirtyBox = !0;
            b !== !1 && d.redraw();
            N(c, f)
        },
        setTooltipPoints: function(a) {
            var b = [],
            c,
            d,
            e = this.xAxis,
            f = e && e.getExtremes(),
            g = e ? e.tooltipLen || e.len: this.chart.plotSizeX,
            h,
            i,
            k = [];
            if (! (this.options.enableMouseTracking === !1 || this.singularTooltips)) {
                if (a) this.tooltipPoints = null;
                q(this.segments || this.points,
                function(a) {
                    b = b.concat(a)
                });
                e && e.reversed && (b = b.reverse());
                this.orderTooltipPoints && this.orderTooltipPoints(b);
                a = b.length;
                for (i = 0; i < a; i++) if (e = b[i], c = e.x, c >= f.min && c <= f.max) {
                    h = b[i + 1];
                    c = d === r ? 0 : d + 1;
                    for (d = b[i + 1] ? z(w(0, S((e.clientX + (h ? h.wrappedClientX || h.clientX: g)) / 2)), g) : g; c >= 0 && c <= d;) k[c++] = e
                }
                this.tooltipPoints = k
            }
        },
        show: function() {
            this.setVisible(!0)
        },
        hide: function() {
            this.setVisible(!1)
        },
        select: function(a) {
            this.selected = a = a === r ? !this.selected: a;
            if (this.checkbox) this.checkbox.checked = a;
            N(this, a ? "select": "unselect")
        },
        drawTracker: gb.drawTrackerGraph
    });
    O(K.prototype, "init",
    function(a) {
        var b;
        a.apply(this, Array.prototype.slice.call(arguments, 1)); (b = this.xAxis) && b.options.ordinal && A(this, "updatedData",
        function() {
            delete b.ordinalIndex
        })
    });
    O(L.prototype, "getTimeTicks",
    function(a, b, c, d, e, f, g, h) {
        var i = 0,
        k = 0,
        j, l = {},
        m, n, p, o = [],
        q = -Number.MAX_VALUE,
        s = this.options.tickPixelInterval;
        if (!this.options.ordinal || !f || f.length < 3 || c === r) return a.call(this, b, c, d, e);
        for (n = f.length; k < n; k++) {
            p = k && f[k - 1] > d;
            f[k] < c && (i = k);
            if (k === n - 1 || f[k + 1] - f[k] > g * 5 || p) {
                if (f[k] > q) {
                    for (j = a.call(this, b, f[i], f[k], e); j.length && j[0] <= q;) j.shift();
                    j.length && (q = j[j.length - 1]);
                    o = o.concat(j)
                }
                i = k + 1
            }
            if (p) break
        }
        a = j.info;
        if (h && a.unitRange <= H.hour) {
            k = o.length - 1;
            for (i = 1; i < k; i++)(new Date(o[i] - La))[Ua]() !== (new Date(o[i - 1] - La))[Ua]() && (l[o[i]] = "day", m = !0);
            m && (l[o[0]] = "day");
            a.higherRanks = l
        }
        o.info = a;
        if (h && t(s)) {
            var h = a = o.length,
            k = [],
            x;
            for (m = []; h--;) i = this.translate(o[h]),
            x && (m[h] = x - i),
            k[h] = x = i;
            m.sort();
            m = m[S(m.length / 2)];
            m < s * 0.6 && (m = null);
            h = o[a - 1] > d ? a - 1 : a;
            for (x = void 0; h--;) i = k[h],
            d = x - i,
            x && d < s * 0.8 && (m === null || d < m * 0.8) ? (l[o[h]] && !l[o[h + 1]] ? (d = h + 1, x = i) : d = h, o.splice(d, 1)) : x = i
        }
        return o
    });
    v(L.prototype, {
        beforeSetTickPositions: function() {
            var a, b = [],
            c = !1,
            d,
            e = this.getExtremes(),
            f = e.min,
            e = e.max,
            g;
            if (this.options.ordinal) {
                q(this.series,
                function(c, d) {
                    if (c.visible !== !1 && c.takeOrdinalPosition !== !1 && (b = b.concat(c.processedXData), a = b.length, b.sort(function(a, b) {
                        return a - b
                    }), a)) for (d = a - 1; d--;) b[d] === b[d + 1] && b.splice(d, 1)
                });
                a = b.length;
                if (a > 2) {
                    d = b[1] - b[0];
                    for (g = a - 1; g--&&!c;) b[g + 1] - b[g] !== d && (c = !0);
                    if (!this.options.keepOrdinalPadding && (b[0] - f > d || e - b[b.length - 1] > d)) c = !0
                }
                c ? (this.ordinalPositions = b, c = this.val2lin(w(f, b[0]), !0), d = this.val2lin(z(e, b[b.length - 1]), !0), this.ordinalSlope = e = (e - f) / (d - c), this.ordinalOffset = f - c * e) : this.ordinalPositions = this.ordinalSlope = this.ordinalOffset = r
            }
            this.groupIntervalFactor = null
        },
        val2lin: function(a, b) {
            var c = this.ordinalPositions;
            if (c) {
                var d = c.length,
                e, f;
                for (e = d; e--;) if (c[e] === a) {
                    f = e;
                    break
                }
                for (e = d - 1; e--;) if (a > c[e] || e === 0) {
                    c = (a - c[e]) / (c[e + 1] - c[e]);
                    f = e + c;
                    break
                }
                return b ? f: this.ordinalSlope * (f || 0) + this.ordinalOffset
            } else return a
        },
        lin2val: function(a, b) {
            var c = this.ordinalPositions;
            if (c) {
                var d = this.ordinalSlope,
                e = this.ordinalOffset,
                f = c.length - 1,
                g, h;
                if (b) a < 0 ? a = c[0] : a > f ? a = c[f] : (f = S(a), h = a - f);
                else for (; f--;) if (g = d * f + e, a >= g) {
                    d = d * (f + 1) + e;
                    h = (a - g) / (d - g);
                    break
                }
                return h !== r && c[f] !== r ? c[f] + (h ? h * (c[f + 1] - c[f]) : 0) : a
            } else return a
        },
        getExtendedPositions: function() {
            var a = this.chart,
            b = this.series[0].currentDataGrouping,
            c = this.ordinalIndex,
            d = b ? b.count + b.unitName: "raw",
            e = this.getExtremes(),
            f,
            g;
            if (!c) c = this.ordinalIndex = {};
            if (!c[d]) f = {
                series: [],
                getExtremes: function() {
                    return {
                        min: e.dataMin,
                        max: e.dataMax
                    }
                },
                options: {
                    ordinal: !0
                },
                val2lin: L.prototype.val2lin
            },
            q(this.series,
            function(c) {
                g = {
                    xAxis: f,
                    xData: c.xData,
                    chart: a,
                    destroyGroupedData: na
                };
                g.options = {
                    dataGrouping: b ? {
                        enabled: !0,
                        forced: !0,
                        approximation: "open",
                        units: [[b.unitName, [b.count]]]
                    }: {
                        enabled: !1
                    }
                };
                c.processData.apply(g);
                f.series.push(g)
            }),
            this.beforeSetTickPositions.apply(f),
            c[d] = f.ordinalPositions;
            return c[d]
        },
        getGroupIntervalFactor: function(a, b, c) {
            var d = 0,
            c = c.processedXData,
            e = c.length,
            f = [],
            g = this.groupIntervalFactor;
            if (!g) {
                for (; d < e - 1; d++) f[d] = c[d + 1] - c[d];
                f.sort(function(a, b) {
                    return a - b
                });
                d = f[S(e / 2)];
                a = w(a, c[0]);
                b = z(b, c[e - 1]);
                this.groupIntervalFactor = g = e * d / (b - a)
            }
            return g
        },
        postProcessTickInterval: function(a) {
            var b = this.ordinalSlope;
            return b ? a / (b / this.closestPointRange) : a
        }
    });
    O(va.prototype, "pan",
    function(a, b) {
        var c = this.xAxis[0],
        d = b.chartX,
        e = !1;
        if (c.options.ordinal && c.series.length) {
            var f = this.mouseDownX,
            g = c.getExtremes(),
            h = g.dataMax,
            i = g.min,
            k = g.max,
            j = this.hoverPoints,
            l = c.closestPointRange,
            f = (f - d) / (c.translationSlope * (c.ordinalSlope || l)),
            m = {
                ordinalPositions: c.getExtendedPositions()
            },
            l = c.lin2val,
            n = c.val2lin,
            p;
            if (m.ordinalPositions) {
                if (M(f) > 1) j && q(j,
                function(a) {
                    a.setState()
                }),
                f < 0 ? (j = m, p = c.ordinalPositions ? c: m) : (j = c.ordinalPositions ? c: m, p = m),
                m = p.ordinalPositions,
                h > m[m.length - 1] && m.push(h),
                this.fixedRange = k - i,
                f = c.toFixedRange(null, null, l.apply(j, [n.apply(j, [i, !0]) + f, !0]), l.apply(p, [n.apply(p, [k, !0]) + f, !0])),
                f.min >= z(g.dataMin, i) && f.max <= w(h, k) && c.setExtremes(f.min, f.max, !0, !1, {
                    trigger: "pan"
                }),
                this.mouseDownX = d,
                E(this.container, {
                    cursor: "move"
                })
            } else e = !0
        } else e = !0;
        e && a.apply(this, Array.prototype.slice.call(arguments, 1))
    });
    O(K.prototype, "getSegments",
    function(a) {
        var b, c = this.options.gapSize,
        d = this.xAxis;
        a.apply(this, Array.prototype.slice.call(arguments, 1));
        if (c) b = this.segments,
        q(b,
        function(a, f) {
            for (var g = a.length - 1; g--;) a[g + 1].x - a[g].x > d.closestPointRange * c && b.splice(f + 1, 0, a.splice(g + 1, a.length - g))
        })
    });
    var ba = K.prototype,
    J = Hb.prototype,
    hc = ba.processData,
    ic = ba.generatePoints,
    jc = ba.destroy,
    kc = J.tooltipHeaderFormatter,
    lc = {
        approximation: "average",
        groupPixelWidth: 2,
        dateTimeLabelFormats: Kb("millisecond", ["%A, %b %e, %H:%M:%S.%L", "%A, %b %e, %H:%M:%S.%L", "-%H:%M:%S.%L"], "second", ["%A, %b %e, %H:%M:%S", "%A, %b %e, %H:%M:%S", "-%H:%M:%S"], "minute", ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], "hour", ["%A, %b %e, %H:%M", "%A, %b %e, %H:%M", "-%H:%M"], "day", ["%A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], "week", ["Week from %A, %b %e, %Y", "%A, %b %e", "-%A, %b %e, %Y"], "month", ["%B %Y", "%B", "-%B %Y"], "year", ["%Y", "%Y", "-%Y"])
    },
    Wb = {
        line: {},
        spline: {},
        area: {},
        areaspline: {},
        column: {
            approximation: "sum",
            groupPixelWidth: 10
        },
        arearange: {
            approximation: "range"
        },
        areasplinerange: {
            approximation: "range"
        },
        columnrange: {
            approximation: "range",
            groupPixelWidth: 10
        },
        candlestick: {
            approximation: "ohlc",
            groupPixelWidth: 10
        },
        ohlc: {
            approximation: "ohlc",
            groupPixelWidth: 5
        }
    },
    Xb = [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1]], ["week", [1]], ["month", [1, 3, 6]], ["year", null]],
    Na = {
        sum: function(a) {
            var b = a.length,
            c;
            if (!b && a.hasNulls) c = null;
            else if (b) for (c = 0; b--;) c += a[b];
            return c
        },
        average: function(a) {
            var b = a.length,
            a = Na.sum(a);
            typeof a === "number" && b && (a /= b);
            return a
        },
        open: function(a) {
            return a.length ? a[0] : a.hasNulls ? null: r
        },
        high: function(a) {
            return a.length ? Ba(a) : a.hasNulls ? null: r
        },
        low: function(a) {
            return a.length ? Ra(a) : a.hasNulls ? null: r
        },
        close: function(a) {
            return a.length ? a[a.length - 1] : a.hasNulls ? null: r
        },
        ohlc: function(a, b, c, d) {
            a = Na.open(a);
            b = Na.high(b);
            c = Na.low(c);
            d = Na.close(d);
            if (typeof a === "number" || typeof b === "number" || typeof c === "number" || typeof d === "number") return [a, b, c, d]
        },
        range: function(a, b) {
            a = Na.low(a);
            b = Na.high(b);
            if (typeof a === "number" || typeof b === "number") return [a, b]
        }
    };
    ba.groupData = function(a, b, c, d) {
        var e = this.data,
        f = this.options.data,
        g = [],
        h = [],
        i = a.length,
        k,
        j,
        l = !!b,
        m = [[], [], [], []],
        d = typeof d === "function" ? d: Na[d],
        n = this.pointArrayMap,
        p = n && n.length,
        o;
        for (o = 0; o <= i; o++) if (a[o] >= c[0]) break;
        for (; o <= i; o++) {
            for (; c[1] !== r && a[o] >= c[1] || o === i;) if (k = c.shift(), j = d.apply(0, m), j !== r && (g.push(k), h.push(j)), m[0] = [], m[1] = [], m[2] = [], m[3] = [], o === i) break;
            if (o === i) break;
            if (n) {
                k = this.cropStart + o;
                k = e && e[k] || this.pointClass.prototype.applyOptions.apply({
                    series: this
                },
                [f[k]]);
                var q;
                for (j = 0; j < p; j++) if (q = k[n[j]], typeof q === "number") m[j].push(q);
                else if (q === null) m[j].hasNulls = !0
            } else if (k = l ? b[o] : null, typeof k === "number") m[0].push(k);
            else if (k === null) m[0].hasNulls = !0
        }
        return [g, h]
    };
    ba.processData = function() {
        var a = this.chart,
        b = this.options,
        c = b.dataGrouping,
        d = c && o(c.enabled, a.options._stock),
        e;
        this.forceCrop = d;
        this.groupPixelWidth = null;
        this.hasProcessed = !0;
        if (hc.apply(this, arguments) !== !1 && d) {
            this.destroyGroupedData();
            var f = this.processedXData,
            g = this.processedYData,
            h = a.plotSizeX,
            a = this.xAxis,
            i = a.options.ordinal,
            k = this.groupPixelWidth = a.getGroupPixelWidth && a.getGroupPixelWidth(),
            d = this.pointRange;
            if (k) {
                e = !0;
                this.points = null;
                var j = a.getExtremes(),
                d = j.min,
                j = j.max,
                i = i && a.getGroupIntervalFactor(d, j, this) || 1,
                h = k * (j - d) / h * i,
                k = a.getTimeTicks(a.normalizeTimeTickInterval(h, c.units || Xb), d, j, null, f, this.closestPointRange),
                g = ba.groupData.apply(this, [f, g, k, c.approximation]),
                f = g[0],
                g = g[1];
                if (c.smoothed) {
                    c = f.length - 1;
                    for (f[c] = j; c--&&c > 0;) f[c] += h / 2;
                    f[0] = d
                }
                this.currentDataGrouping = k.info;
                if (b.pointRange === null) this.pointRange = k.info.totalRange;
                this.closestPointRange = k.info.totalRange;
                if (t(f[0]) && f[0] < a.dataMin) a.dataMin = f[0];
                this.processedXData = f;
                this.processedYData = g
            } else this.currentDataGrouping = null,
            this.pointRange = d;
            this.hasGroupedData = e
        }
    };
    ba.destroyGroupedData = function() {
        var a = this.groupedData;
        q(a || [],
        function(b, c) {
            b && (a[c] = b.destroy ? b.destroy() : null)
        });
        this.groupedData = null
    };
    ba.generatePoints = function() {
        ic.apply(this);
        this.destroyGroupedData();
        this.groupedData = this.hasGroupedData ? this.points: null
    };
    J.tooltipHeaderFormatter = function(a) {
        var b = a.series,
        c = b.tooltipOptions,
        d = b.options.dataGrouping,
        e = c.xDateFormat,
        f, g = b.xAxis,
        h;
        if (g && g.options.type === "datetime" && d && la(a.key)) {
            b = b.currentDataGrouping;
            d = d.dateTimeLabelFormats;
            if (b) g = d[b.unitName],
            b.count === 1 ? e = g[0] : (e = g[1], f = g[2]);
            else if (!e && d) for (h in H) if (H[h] >= g.closestPointRange || H[h] <= H.day && a.key % H[h] > 0) {
                e = d[h][0];
                break
            }
            e = ua(e, a.key);
            f && (e += ua(f, a.key + b.totalRange - 1));
            a = c.headerFormat.replace("{point.key}", e)
        } else a = kc.call(this, a);
        return a
    };
    ba.destroy = function() {
        for (var a = this.groupedData || [], b = a.length; b--;) a[b] && a[b].destroy();
        jc.apply(this)
    };
    O(ba, "setOptions",
    function(a, b) {
        var c = a.call(this, b),
        d = this.type,
        e = this.chart.options.plotOptions,
        f = T[d].dataGrouping;
        if (Wb[d]) f || (f = y(lc, Wb[d])),
        c.dataGrouping = y(f, e.series && e.series.dataGrouping, e[d].dataGrouping, b.dataGrouping);
        if (this.chart.options._stock) this.requireSorting = !0;
        return c
    });
    O(L.prototype, "setScale",
    function(a) {
        a.call(this);
        q(this.series,
        function(a) {
            a.hasProcessed = !1
        })
    });
    L.prototype.getGroupPixelWidth = function() {
        var a = this.series,
        b = a.length,
        c, d = 0,
        e = !1,
        f;
        for (c = b; c--;)(f = a[c].options.dataGrouping) && (d = w(d, f.groupPixelWidth));
        for (c = b; c--;) if ((f = a[c].options.dataGrouping) && a[c].hasProcessed) if (b = (a[c].processedXData || a[c].data).length, a[c].groupPixelWidth || b > this.chart.plotSizeX / d || b && f.forced) e = !0;
        return e ? d: 0
    };
    T.ohlc = y(T.column, {
        lineWidth: 1,
        tooltip: {
            pointFormat: '<span style="color:{series.color}">●</span> <b> {series.name}</b><br/>Open: {point.open}<br/>High: {point.high}<br/>Low: {point.low}<br/>Close: {point.close}<br/>'
        },
        states: {
            hover: {
                lineWidth: 3
            }
        },
        threshold: null
    });
    J = ga(C.column, {
        type: "ohlc",
        pointArrayMap: ["open", "high", "low", "close"],
        toYData: function(a) {
            return [a.open, a.high, a.low, a.close]
        },
        pointValKey: "high",
        pointAttrToOptions: {
            stroke: "color",
            "stroke-width": "lineWidth"
        },
        upColorProp: "stroke",
        getAttribs: function() {
            C.column.prototype.getAttribs.apply(this, arguments);
            var a = this.options,
            b = a.states,
            a = a.upColor || this.color,
            c = y(this.pointAttr),
            d = this.upColorProp;
            c[""][d] = a;
            c.hover[d] = b.hover.upColor || a;
            c.select[d] = b.select.upColor || a;
            q(this.points,
            function(a) {
                if (a.open < a.close) a.pointAttr = c
            })
        },
        translate: function() {
            var a = this.yAxis;
            C.column.prototype.translate.apply(this);
            q(this.points,
            function(b) {
                if (b.open !== null) b.plotOpen = a.translate(b.open, 0, 1, 0, 1);
                if (b.close !== null) b.plotClose = a.translate(b.close, 0, 1, 0, 1)
            })
        },
        drawPoints: function() {
            var a = this,
            b = a.chart,
            c, d, e, f, g, h, i, k;
            q(a.points,
            function(j) {
                if (j.plotY !== r) i = j.graphic,
                c = j.pointAttr[j.selected ? "selected": ""] || a.pointAttr[""],
                f = c["stroke-width"] % 2 / 2,
                k = s(j.plotX) - f,
                g = s(j.shapeArgs.width / 2),
                h = ["M", k, s(j.yBottom), "L", k, s(j.plotY)],
                j.open !== null && (d = s(j.plotOpen) + f, h.push("M", k, d, "L", k - g, d)),
                j.close !== null && (e = s(j.plotClose) + f, h.push("M", k, e, "L", k + g, e)),
                i ? i.animate({
                    d: h
                }) : j.graphic = b.renderer.path(h).attr(c).add(a.group)
            })
        },
        animate: null
    });
    C.ohlc = J;
    T.candlestick = y(T.column, {
        lineColor: "black",
        lineWidth: 1,
        states: {
            hover: {
                lineWidth: 2
            }
        },
        tooltip: T.ohlc.tooltip,
        threshold: null,
        upColor: "white"
    });
    J = ga(J, {
        type: "candlestick",
        pointAttrToOptions: {
            fill: "color",
            stroke: "lineColor",
            "stroke-width": "lineWidth"
        },
        upColorProp: "fill",
        getAttribs: function() {
            C.ohlc.prototype.getAttribs.apply(this, arguments);
            var a = this.options,
            b = a.states,
            c = a.upLineColor || a.lineColor,
            d = b.hover.upLineColor || c,
            e = b.select.upLineColor || c;
            q(this.points,
            function(a) {
                if (a.open < a.close) a.pointAttr[""].stroke = c,
                a.pointAttr.hover.stroke = d,
                a.pointAttr.select.stroke = e
            })
        },
        drawPoints: function() {
            var a = this,
            b = a.chart,
            c, d = a.pointAttr[""],
            e,
            f,
            g,
            h,
            i,
            k,
            j,
            l,
            m,
            n,
            p;
            q(a.points,
            function(o) {
                m = o.graphic;
                if (o.plotY !== r) c = o.pointAttr[o.selected ? "selected": ""] || d,
                j = c["stroke-width"] % 2 / 2,
                l = s(o.plotX) - j,
                e = o.plotOpen,
                f = o.plotClose,
                g = V.min(e, f),
                h = V.max(e, f),
                p = s(o.shapeArgs.width / 2),
                i = s(g) !== s(o.plotY),
                k = h !== o.yBottom,
                g = s(g) + j,
                h = s(h) + j,
                n = ["M", l - p, h, "L", l - p, g, "L", l + p, g, "L", l + p, h, "Z", "M", l, g, "L", l, i ? s(o.plotY) : g, "M", l, h, "L", l, k ? s(o.yBottom) : h, "Z"],
                m ? m.animate({
                    d: n
                }) : o.graphic = b.renderer.path(n).attr(c).add(a.group).shadow(a.options.shadow)
            })
        }
    });
    C.candlestick = J;
    var qb = ka.prototype.symbols;
    T.flags = y(T.column, {
        dataGrouping: null,
        fillColor: "white",
        lineWidth: 1,
        pointRange: 0,
        shape: "flag",
        stackDistance: 12,
        states: {
            hover: {
                lineColor: "black",
                fillColor: "#FCFFC5"
            }
        },
        style: {
            fontSize: "11px",
            fontWeight: "bold",
            textAlign: "center"
        },
        tooltip: {
            pointFormat: "{point.text}<br/>"
        },
        threshold: null,
        y: -30
    });
    C.flags = ga(C.column, {
        type: "flags",
        sorted: !1,
        noSharedTooltip: !0,
        takeOrdinalPosition: !1,
        trackerGroups: ["markerGroup"],
        forceCrop: !0,
        init: K.prototype.init,
        pointAttrToOptions: {
            fill: "fillColor",
            stroke: "color",
            "stroke-width": "lineWidth",
            r: "radius"
        },
        translate: function() {
            C.column.prototype.translate.apply(this);
            var a = this.chart,
            b = this.points,
            c = b.length - 1,
            d, e, f = this.options.onSeries,
            f = (d = f && a.get(f)) && d.options.step,
            g = d && d.points,
            h = g && g.length,
            i = this.xAxis,
            k = i.getExtremes(),
            j,
            l,
            m;
            if (d && d.visible && h) {
                d = d.currentDataGrouping;
                l = g[h - 1].x + (d ? d.totalRange: 0);
                for (b.sort(function(a, b) {
                    return a.x - b.x
                }); h--&&b[c];) if (d = b[c], j = g[h], j.x <= d.x && j.plotY !== r) {
                    if (d.x <= l) d.plotY = j.plotY,
                    j.x < d.x && !f && (m = g[h + 1]) && m.plotY !== r && (d.plotY += (d.x - j.x) / (m.x - j.x) * (m.plotY - j.plotY));
                    c--;
                    h++;
                    if (c < 0) break
                }
            }
            q(b,
            function(c, d) {
                if (c.plotY === r) c.x >= k.min && c.x <= k.max ? c.plotY = a.chartHeight - i.bottom - (i.opposite ? i.height: 0) + i.offset - a.plotTop: c.shapeArgs = {};
                if ((e = b[d - 1]) && e.plotX === c.plotX) {
                    if (e.stackIndex === r) e.stackIndex = 0;
                    c.stackIndex = e.stackIndex + 1
                }
            })
        },
        drawPoints: function() {
            var a, b = this.pointAttr[""],
            c = this.points,
            d = this.chart.renderer,
            e,
            f,
            g = this.options,
            h = g.y,
            i,
            k,
            j,
            l,
            m = g.lineWidth % 2 / 2,
            n,
            o;
            for (k = c.length; k--;) if (j = c[k], a = j.plotX > this.xAxis.len, e = j.plotX + (a ? m: -m), l = j.stackIndex, i = j.options.shape || g.shape, f = j.plotY, f !== r && (f = j.plotY + h + m - (l !== r && l * g.stackDistance)), n = l ? r: j.plotX + m, o = l ? r: j.plotY, l = j.graphic, f !== r && e >= 0 && !a) a = j.pointAttr[j.selected ? "select": ""] || b,
            l ? l.attr({
                x: e,
                y: f,
                r: a.r,
                anchorX: n,
                anchorY: o
            }) : j.graphic = d.label(j.options.title || g.title || "A", e, f, i, n, o, g.useHTML).css(y(g.style, j.style)).attr(a).attr({
                align: i === "flag" ? "left": "center",
                width: g.width,
                height: g.height
            }).add(this.markerGroup).shadow(g.shadow),
            j.tooltipPos = [e, f];
            else if (l) j.graphic = l.destroy()
        },
        drawTracker: function() {
            var a = this.points;
            gb.drawTrackerPoint.apply(this);
            q(a,
            function(b) {
                var c = b.graphic;
                c && A(c.element, "mouseover",
                function() {
                    if (b.stackIndex > 0 && !b.raised) b._y = c.y,
                    c.attr({
                        y: b._y - 8
                    }),
                    b.raised = !0;
                    q(a,
                    function(a) {
                        if (a !== b && a.raised && a.graphic) a.graphic.attr({
                            y: a._y
                        }),
                        a.raised = !1
                    })
                })
            })
        },
        animate: na
    });
    qb.flag = function(a, b, c, d, e) {
        var f = e && e.anchorX || a,
        e = e && e.anchorY || b;
        return ["M", f, e, "L", a, b + d, a, b, a + c, b, a + c, b + d, a, b + d, "M", f, e, "Z"]
    };
    q(["circle", "square"],
    function(a) {
        qb[a + "pin"] = function(b, c, d, e, f) {
            var g = f && f.anchorX,
            f = f && f.anchorY,
            b = qb[a](b, c, d, e);
            g && f && b.push("M", g, c > f ? c: c + e, "L", g, f);
            return b
        }
    });
    Wa === P.VMLRenderer && q(["flag", "circlepin", "squarepin"],
    function(a) {
        fb.prototype.symbols[a] = qb[a]
    });
    J = [].concat(Xb);
    J[4] = ["day", [1, 2, 3, 4]];
    J[5] = ["week", [1, 2, 3]];
    v(F, {
        navigator: {
            handles: {
                backgroundColor: "#ebe7e8",
                borderColor: "#b2b1b6"
            },
            height: 40,
            margin: 25,
            maskFill: "rgba(128,179,236,0.3)",
            maskInside: !0,
            outlineColor: "#666",
            outlineWidth: 1,
            series: {
                type: C.areaspline === r ? "line": "areaspline",
                color: "#4572A7",
                compare: null,
                fillOpacity: 0.05,
                dataGrouping: {
                    approximation: "average",
                    enabled: !0,
                    groupPixelWidth: 2,
                    smoothed: !0,
                    units: J
                },
                dataLabels: {
                    enabled: !1,
                    zIndex: 2
                },
                id: "highcharts-navigator-series",
                lineColor: "#4572A7",
                lineWidth: 1,
                marker: {
                    enabled: !1
                },
                pointRange: 0,
                shadow: !1,
                threshold: null
            },
            xAxis: {
                tickWidth: 0,
                lineWidth: 0,
                gridLineColor: "#666",
                gridLineWidth: 1,
                tickPixelInterval: 200,
                labels: {
                    align: "left",
                    style: {
                        color: "#888"
                    },
                    x: 3,
                    y: -4
                },
                crosshair: !1
            },
            yAxis: {
                gridLineWidth: 0,
                startOnTick: !1,
                endOnTick: !1,
                minPadding: 0.1,
                maxPadding: 0.1,
                labels: {
                    enabled: !1
                },
                crosshair: !1,
                title: {
                    text: null
                },
                tickWidth: 0
            }
        },
        scrollbar: {
            height: cb ? 20 : 14,
            barBackgroundColor: 'gray',
            barBorderRadius: 0,
            barBorderWidth: 0,
            buttonBackgroundColor: 'gray',
            buttonBorderWidth: 0,
            buttonArrowColor: 'yellow',
            buttonBorderRadius: 7,
            rifleColor: 'yellow',
            trackBackgroundColor: '#0a0a0a',
            trackBorderWidth: 0,
            trackBorderColor: 'silver',
            trackBorderRadius: 7,
            //enabled: false,
            liveRedraw: false //设置scrollbar在移动过程中，chart不会重绘
        }
    });
    yb.prototype = {
        drawHandle: function(a, b) {
            var c = this.chart,
            d = c.renderer,
            e = this.elementsToDestroy,
            f = this.handles,
            g = this.navigatorOptions.handles,
            g = {
                fill: g.backgroundColor,
                stroke: g.borderColor,
                "stroke-width": 1
            },
            h;
            this.rendered || (f[b] = d.g("navigator-handle-" + ["left", "right"][b]).css({
                cursor: "e-resize"
            }).attr({
                zIndex: 4 - b
            }).add(), h = d.rect( - 4.5, 0, 9, 16, 0, 1).attr(g).add(f[b]), e.push(h), h = d.path(["M", -1.5, 4, "L", -1.5, 12, "M", 0.5, 4, "L", 0.5, 12]).attr(g).add(f[b]), e.push(h));
            f[b][c.isResizing ? "animate": "attr"]({
                translateX: this.scrollerLeft + this.scrollbarHeight + parseInt(a, 10),
                translateY: this.top + this.height / 2 - 8
            })
        },
        drawScrollbarButton: function(a) {
            var b = this.chart.renderer,
            c = this.elementsToDestroy,
            d = this.scrollbarButtons,
            e = this.scrollbarHeight,
            f = this.scrollbarOptions,
            g;
            this.rendered || (d[a] = b.g().add(this.scrollbarGroup), g = b.rect( - 0.5, -0.5, e + 1, e + 1, f.buttonBorderRadius, f.buttonBorderWidth).attr({
                stroke: f.buttonBorderColor,
                "stroke-width": f.buttonBorderWidth,
                fill: f.buttonBackgroundColor
            }).add(d[a]), c.push(g), g = b.path(["M", e / 2 + (a ? -1 : 1), e / 2 - 3, "L", e / 2 + (a ? -1 : 1), e / 2 + 3, e / 2 + (a ? 2 : -2), e / 2]).attr({
                fill: f.buttonArrowColor
            }).add(d[a]), c.push(g));
            a && d[a].attr({
                translateX: this.scrollerWidth - e
            })
        },
        render: function(a, b, c, d) {
            var e = this.chart,
            f = e.renderer,
            g, h, i, k, j = this.scrollbarGroup,
            l = this.navigatorGroup,
            m = this.scrollbar,
            l = this.xAxis,
            n = this.scrollbarTrack,
            p = this.scrollbarHeight,
            q = this.scrollbarEnabled,
            r = this.navigatorOptions,
            t = this.scrollbarOptions,
            x = t.minWidth,
            v = this.height,
            y = this.top,
            A = this.navigatorEnabled,
            B = r.outlineWidth,
            C = B / 2,
            F = 0,
            D = this.outlineHeight,
            I = t.barBorderRadius,
            H = t.barBorderWidth,
            E = y + C,
            J;
            if (!isNaN(a)) {
                this.navigatorLeft = g = o(l.left, e.plotLeft + p);
                this.navigatorWidth = h = o(l.len, e.plotWidth - 2 * p);
                this.scrollerLeft = i = g - p;
                this.scrollerWidth = k = k = h + 2 * p;
                l.getExtremes && (J = this.getUnionExtremes(!0)) && (J.dataMin !== l.min || J.dataMax !== l.max) && l.setExtremes(J.dataMin, J.dataMax, !0, !1);
                c = o(c, l.translate(a));
                d = o(d, l.translate(b));
                if (isNaN(c) || M(c) === Infinity) c = 0,
                d = k;
                if (! (l.translate(d, !0) - l.translate(c, !0) < e.xAxis[0].minRange)) {
                    this.zoomedMax = z(w(c, d), h);
                    this.zoomedMin = w(this.fixedWidth ? this.zoomedMax - this.fixedWidth: z(c, d), 0);
                    this.range = this.zoomedMax - this.zoomedMin;
                    c = s(this.zoomedMax);
                    b = s(this.zoomedMin);
                    a = c - b;
                    if (!this.rendered) {
                        if (A) {
                            this.navigatorGroup = l = f.g("navigator").attr({
                                zIndex: 3
                            }).add();
                            this.leftShade = f.rect().attr({
                                fill: r.maskFill
                            }).add(l);
                            if (!r.maskInside) this.rightShade = f.rect().attr({
                                fill: r.maskFill
                            }).add(l);
                            this.outline = f.path().attr({
                                "stroke-width": B,
                                stroke: r.outlineColor
                            }).add(l)
                        }
                        if (q) this.scrollbarGroup = j = f.g("scrollbar").add(),
                        m = t.trackBorderWidth,
                        this.scrollbarTrack = n = f.rect().attr({
                            x: 0,
                            y: -m % 2 / 2,
                            fill: t.trackBackgroundColor,
                            stroke: t.trackBorderColor,
                            "stroke-width": m,
                            r: t.trackBorderRadius || 0,
                            height: p
                        }).add(j),
                        this.scrollbar = m = f.rect().attr({
                            y: -H % 2 / 2,
                            height: p,
                            fill: t.barBackgroundColor,
                            stroke: t.barBorderColor,
                            "stroke-width": H,
                            r: I
                        }).add(j),
                        this.scrollbarRifles = f.path().attr({
                            stroke: t.rifleColor,
                            "stroke-width": 1
                        }).add(j)
                    }
                    e = e.isResizing ? "animate": "attr";
                    if (A) {
                        this.leftShade[e](r.maskInside ? {
                            x: g + b,
                            y: y,
                            width: c - b,
                            height: v
                        }: {
                            x: g,
                            y: y,
                            width: b,
                            height: v
                        });
                        if (this.rightShade) this.rightShade[e]({
                            x: g + c,
                            y: y,
                            width: h - c,
                            height: v
                        });
                        this.outline[e]({
                            d: ["M", i, E, "L", g + b + C, E, g + b + C, E + D, "L", g + c - C, E + D, "L", g + c - C, E, i + k, E].concat(r.maskInside ? ["M", g + b + C, E, "L", g + c - C, E] : [])
                        });
                        this.drawHandle(b + C, 0);
                        this.drawHandle(c + C, 1)
                    }
                    if (q && j) this.drawScrollbarButton(0),
                    this.drawScrollbarButton(1),
                    j[e]({
                        translateX: i,
                        translateY: s(E + v)
                    }),
                    n[e]({
                        width: k
                    }),
                    g = p + b,
                    h = a - H,
                    h < x && (F = (x - h) / 2, h = x, g -= F),
                    this.scrollbarPad = F,
                    m[e]({
                        x: S(g) + H % 2 / 2,
                        width: h
                    }),
                    x = p + b + a / 2 - 0.5,
                    this.scrollbarRifles.attr({
                        visibility: a > 12 ? "visible": "hidden"
                    })[e]({
                        d: ["M", x - 3, p / 4, "L", x - 3, 2 * p / 3, "M", x, p / 4, "L", x, 2 * p / 3, "M", x + 3, p / 4, "L", x + 3, 2 * p / 3]
                    });
                    this.scrollbarPad = F;
                    this.rendered = !0
                }
            }
        },
        addEvents: function() {
            var a = this.chart.container,
            b = this.mouseDownHandler,
            c = this.mouseMoveHandler,
            d = this.mouseUpHandler,
            e;
            e = [[a, "mousedown", b], [a, "mousemove", c], [document, "mouseup", d]];
            ab && e.push([a, "touchstart", b], [a, "touchmove", c], [document, "touchend", d]);
            q(e,
            function(a) {
                A.apply(null, a)
            });
            this._events = e
        },
        removeEvents: function() {
            q(this._events,
            function(a) {
                R.apply(null, a)
            });
            this._events = r;
            this.navigatorEnabled && this.baseSeries && R(this.baseSeries, "updatedData", this.updatedDataHandler)
        },
        init: function() {
            var a = this,
            b = a.chart,
            c, d, e = a.scrollbarHeight,
            f = a.navigatorOptions,
            g = a.height,
            h = a.top,
            i, k, j = document.body.style,
            l, m = a.baseSeries;
            a.mouseDownHandler = function(d) {
                var d = b.pointer.normalize(d),
                e = a.zoomedMin,
                f = a.zoomedMax,
                h = a.top,
                k = a.scrollbarHeight,
                m = a.scrollerLeft,
                n = a.scrollerWidth,
                o = a.navigatorLeft,
                p = a.navigatorWidth,
                q = a.scrollbarPad,
                r = a.range,
                s = d.chartX,
                t = d.chartY,
                d = b.xAxis[0],
                v,
                w = cb ? 10 : 7;
                if (t > h && t < h + g + k) if ((h = !a.scrollbarEnabled || t < h + g) && V.abs(s - e - o) < w) a.grabbedLeft = !0,
                a.otherHandlePos = f,
                a.fixedExtreme = d.max,
                b.fixedRange = null;
                else if (h && V.abs(s - f - o) < w) a.grabbedRight = !0,
                a.otherHandlePos = e,
                a.fixedExtreme = d.min,
                b.fixedRange = null;
                else if (s > o + e - q && s < o + f + q) {
                    a.grabbedCenter = s;
                    a.fixedWidth = r;
                    if (b.renderer.isSVG) l = j.cursor,
                    j.cursor = "ew-resize";
                    i = s - e
                } else if (s > m && s < m + n) {
                    f = h ? s - o - r / 2 : s < o ? e - r * 0.2 : s > m + n - k ? e + r * 0.2 : s < o + e ? e - r: f;
                    if (f < 0) f = 0;
                    else if (f + r >= p) f = p - r,
                    v = c.dataMax;
                    if (f !== e) a.fixedWidth = r,
                    e = c.toFixedRange(f, f + r, null, v),
                    d.setExtremes(e.min, e.max, !0, !1, {
                        trigger: "navigator"
                    })
                }
            };
            a.mouseMoveHandler = function(c) {
                var d = a.scrollbarHeight,
                e = a.navigatorLeft,
                f = a.navigatorWidth,
                g = a.scrollerLeft,
                h = a.scrollerWidth,
                j = a.range,
                l;
                if (c.pageX !== 0) c = b.pointer.normalize(c),
                l = c.chartX,
                l < e ? l = e: l > g + h - d && (l = g + h - d),
                a.grabbedLeft ? (k = !0, a.render(0, 0, l - e, a.otherHandlePos)) : a.grabbedRight ? (k = !0, a.render(0, 0, a.otherHandlePos, l - e)) : a.grabbedCenter && (k = !0, l < i ? l = i: l > f + i - j && (l = f + i - j), a.render(0, 0, l - i, l - i + j)),
                k && a.scrollbarOptions.liveRedraw && setTimeout(function() {
                    a.mouseUpHandler(c)
                },
                0)
            };
            a.mouseUpHandler = function(d) {
                var e, f;
                if (k) {
                    if (a.zoomedMin === a.otherHandlePos) e = a.fixedExtreme;
                    else if (a.zoomedMax === a.otherHandlePos) f = a.fixedExtreme;
                    e = c.toFixedRange(a.zoomedMin, a.zoomedMax, e, f);
                    b.xAxis[0].setExtremes(e.min, e.max, !0, !1, {
                        trigger: "navigator",
                        triggerOp: "navigator-drag",
                        DOMEvent: d
                    })
                }
                if (d.type !== "mousemove") a.grabbedLeft = a.grabbedRight = a.grabbedCenter = a.fixedWidth = a.fixedExtreme = a.otherHandlePos = k = i = null,
                j.cursor = l || ""
            };
            var n = b.xAxis.length,
            p = b.yAxis.length;
            b.extraBottomMargin = a.outlineHeight + f.margin;
            a.navigatorEnabled ? (a.xAxis = c = new L(b, y({
                ordinal: m && m.xAxis.options.ordinal
            },
            f.xAxis, {
                id: "navigator-x-axis",
                isX: !0,
                type: "datetime",
                index: n,
                height: g,
                offset: 0,
                offsetLeft: e,
                offsetRight: -e,
                keepOrdinalPadding: !0,
                startOnTick: !1,
                endOnTick: !1,
                minPadding: 0,
                maxPadding: 0,
                zoomEnabled: !1
            })), a.yAxis = d = new L(b, y(f.yAxis, {
                id: "navigator-y-axis",
                alignTicks: !1,
                height: g,
                offset: 0,
                index: p,
                zoomEnabled: !1
            })), m || f.series.data ? a.addBaseSeries() : b.series.length === 0 && O(b, "redraw",
            function(c, d) {
                if (b.series.length > 0 && !a.series) a.setBaseSeries(),
                b.redraw = c;
                c.call(b, d)
            })) : a.xAxis = c = {
                translate: function(a, c) {
                    var d = b.xAxis[0].getExtremes(),
                    f = b.plotWidth - 2 * e,
                    g = d.dataMin,
                    d = d.dataMax - g;
                    return c ? a * d / f + g: f * (a - g) / d
                },
                toFixedRange: L.prototype.toFixedRange
            };
            O(b, "getMargins",
            function(b) {
                var e = this.legend,
                f = e.options;
                b.call(this);
                a.top = h = a.navigatorOptions.top || this.chartHeight - a.height - a.scrollbarHeight - this.spacing[2] - (f.verticalAlign === "bottom" && f.enabled && !f.floating ? e.legendHeight + o(f.margin, 10) : 0);
                if (c && d) c.options.top = d.options.top = h,
                c.setAxisSize(),
                d.setAxisSize()
            });
            a.addEvents()
        },
        getUnionExtremes: function(a) {
            var b = this.chart.xAxis[0],
            c = this.xAxis,
            d = c.options;
            if (!a || b.dataMin !== null) return {
                dataMin: o(d && d.min, (t(b.dataMin) && t(c.dataMin) ? z: o)(b.dataMin, c.dataMin)),
                dataMax: o(d && d.max, (t(b.dataMax) && t(c.dataMax) ? w: o)(b.dataMax, c.dataMax))
            }
        },
        setBaseSeries: function(a) {
            var b = this.chart,
            a = a || b.options.navigator.baseSeries;
            this.series && this.series.remove();
            this.baseSeries = b.series[a] || typeof a === "string" && b.get(a) || b.series[0];
            this.xAxis && this.addBaseSeries()
        },
        addBaseSeries: function() {
            var a = this.baseSeries,
            b = a ? a.options: {},
            c = b.data,
            d = this.navigatorOptions.series,
            e;
            e = d.data;
            this.hasNavigatorData = !!e;
            b = y(b, d, {
                enableMouseTracking: !1,
                group: "nav",
                padXAxis: !1,
                xAxis: "navigator-x-axis",
                yAxis: "navigator-y-axis",
                name: "Navigator",
                showInLegend: !1,
                isInternal: !0,
                visible: !0
            });
            b.data = e || c;
            this.series = this.chart.initSeries(b);
            if (a && this.navigatorOptions.adaptToUpdatedData !== !1) A(a, "updatedData", this.updatedDataHandler),
            a.userOptions.events = v(a.userOptions.event, {
                updatedData: this.updatedDataHandler
            })
        },
        updatedDataHandler: function() {
            var a = this.chart.scroller,
            b = a.baseSeries,
            c = b.xAxis,
            d = c.getExtremes(),
            e = d.min,
            f = d.max,
            g = d.dataMin,
            d = d.dataMax,
            h = f - e,
            i,
            k,
            j,
            l,
            m,
            n = a.series;
            i = n.xData;
            var o = !!c.setExtremes;
            k = f >= i[i.length - 1] - (this.closestPointRange || 0);
            i = e <= g;
            if (!a.hasNavigatorData) n.options.pointStart = b.xData[0],
            n.setData(b.options.data, !1),
            m = !0;
            i && (l = g, j = l + h);
            k && (j = d, i || (l = w(j - h, n.xData[0])));
            o && (i || k) ? isNaN(l) || c.setExtremes(l, j, !0, !1, {
                trigger: "updatedData"
            }) : (m && this.chart.redraw(!1), a.render(w(e, g), z(f, d)))
        },
        destroy: function() {
            this.removeEvents();
            q([this.xAxis, this.yAxis, this.leftShade, this.rightShade, this.outline, this.scrollbarTrack, this.scrollbarRifles, this.scrollbarGroup, this.scrollbar],
            function(a) {
                a && a.destroy && a.destroy()
            });
            this.xAxis = this.yAxis = this.leftShade = this.rightShade = this.outline = this.scrollbarTrack = this.scrollbarRifles = this.scrollbarGroup = this.scrollbar = null;
            q([this.scrollbarButtons, this.handles, this.elementsToDestroy],
            function(a) {
                Ka(a)
            })
        }
    };
    P.Scroller = yb;
    O(L.prototype, "zoom",
    function(a, b, c) {
        var d = this.chart,
        e = d.options,
        f = e.chart.zoomType,
        g = e.navigator,
        e = e.rangeSelector,
        h;
        if (this.isXAxis && (g && g.enabled || e && e.enabled)) if (f === "x") d.resetZoomButton = "blocked";
        else if (f === "y") h = !1;
        else if (f === "xy") d = this.previousZoom,
        t(b) ? this.previousZoom = [this.min, this.max] : d && (b = d[0], c = d[1], delete this.previousZoom);
        return h !== r ? h: a.call(this, b, c)
    });
    O(va.prototype, "init",
    function(a, b, c) {
        A(this, "beforeRender",
        function() {
            var a = this.options;
            if (a.navigator.enabled || a.scrollbar.enabled) this.scroller = new yb(this)
        });
        a.call(this, b, c)
    });
    O(K.prototype, "addPoint",
    function(a, b, c, d, e) {
        var f = this.options.turboThreshold;
        f && this.xData.length > f && fa(b) && !Pa(b) && this.chart.scroller && qa(20, !0);
        a.call(this, b, c, d, e)
    });
    v(F, {
        rangeSelector: {
            buttonTheme: {
                width: 28,
                height: 18,
                fill: "#0a0a0a",
                padding: 2,
                r: 5,
                "stroke-width": 1,
                style: {
                    color: "#333",
                    cursor: "pointer",
                    fontWeight: "normal"
                },
                zIndex: 7,
                states: {
                    hover: {
                        fill: "#e7e7e7"
                    },
                    select: {
                        fill: "#e7f0f9",
                        style: {
                            color: "black",
                            fontWeight: "bold"
                        }
                    }
                }
            },
            inputPosition: {
                align: "right"
            },
            labelStyle: {
                color: "#666"
            }
        }
    });
    F.lang = y(F.lang, {
        rangeSelectorZoom: "Zoom",
        rangeSelectorFrom: "From",
        rangeSelectorTo: "To"
    });
    zb.prototype = {
        clickButton: function(a, b) {
            var c = this,
            d = c.selected,
            e = c.chart,
            f = c.buttons,
            g = c.buttonOptions[a],
            h = e.xAxis[0],
            i = e.scroller && e.scroller.getUnionExtremes() || h || {},
            k = i.dataMin,
            j = i.dataMax,
            l,
            m = h && s(z(h.max, o(j, h.max))),
            n = new Date(m),
            p = g.type,
            u = g.count,
            i = g._range,
            t;
            if (! (k === null || j === null || a === c.selected)) {
                if (p === "month" || p === "year") l = {
                    month: "Month",
                    year: "FullYear"
                } [p],
                n["set" + l](n["get" + l]() - u),
                l = n.getTime(),
                k = o(k, Number.MIN_VALUE),
                isNaN(l) || l < k ? (l = k, m = z(l + i, j)) : i = m - l;
                else if (i) l = w(m - i, k),
                m = z(l + i, j);
                else if (p === "ytd") if (h) {
                    if (j === r) k = Number.MAX_VALUE,
                    j = Number.MIN_VALUE,
                    q(e.series,
                    function(a) {
                        a = a.xData;
                        k = z(a[0], k);
                        j = w(a[a.length - 1], j)
                    }),
                    b = !1;
                    m = new Date(j);
                    t = m.getFullYear();
                    l = t = w(k || 0, Date.UTC(t, 0, 1));
                    m = m.getTime();
                    m = z(j || m, m)
                } else {
                    A(e, "beforeRender",
                    function() {
                        c.clickButton(a)
                    });
                    return
                } else p === "all" && h && (l = k, m = j);
                f[d] && f[d].setState(0);
                f[a] && f[a].setState(2);
                e.fixedRange = i;
                h ? h.setExtremes(l, m, o(b, 1), 0, {
                    trigger: "rangeSelectorButton",
                    rangeSelectorButton: g
                }) : (d = e.options.xAxis, d[0] = y(d[0], {
                    range: i,
                    min: t
                }));
                c.setSelected(a)
            }
        },
        setSelected: function(a) {
            this.selected = this.options.selected = a
        },
        defaultButtons: [{
            type: "month",
            count: 1,
            text: "1m"
        },
        {
            type: "month",
            count: 3,
            text: "3m"
        },
        {
            type: "month",
            count: 6,
            text: "6m"
        },
        {
            type: "ytd",
            text: "YTD"
        },
        {
            type: "year",
            count: 1,
            text: "1y"
        },
        {
            type: "all",
            text: "All"
        }],
        init: function(a) {
            var b = this,
            c = a.options.rangeSelector,
            d = c.buttons || [].concat(b.defaultButtons),
            e = c.selected,
            f = b.blurInputs = function() {
                var a = b.minInput,
                c = b.maxInput;
                a && a.blur();
                c && c.blur()
            };
            b.chart = a;
            b.options = c;
            b.buttons = [];
            a.extraTopMargin = 35;
            b.buttonOptions = d;
            A(a.container, "mousedown", f);
            A(a, "resize", f);
            q(d, b.computeButtonRange);
            e !== r && d[e] && this.clickButton(e, !1);
            A(a, "load",
            function() {
                A(a.xAxis[0], "afterSetExtremes",
                function() {
                    b.updateButtonStates(!0)
                })
            })
        },
        updateButtonStates: function(a) {
            var b = this,
            c = this.chart,
            d = c.xAxis[0],
            e = c.scroller && c.scroller.getUnionExtremes() || d,
            f = e.dataMin,
            g = e.dataMax,
            h = b.selected,
            i = b.buttons;
            a && c.fixedRange !== s(d.max - d.min) && (i[h] && i[h].setState(0), b.setSelected(null));
            q(b.buttonOptions,
            function(a, c) {
                var e = a._range,
                m = e > g - f,
                n = e < d.minRange,
                o = a.type === "all" && d.max - d.min >= g - f && i[c].state !== 2,
                q = a.type === "ytd" && ua("%Y", f) === ua("%Y", g);
                e === s(d.max - d.min) && c !== h ? (b.setSelected(c), i[c].setState(2)) : m || n || o || q ? i[c].setState(3) : i[c].state === 3 && i[c].setState(0)
            })
        },
        computeButtonRange: function(a) {
            var b = a.type,
            c = a.count || 1,
            d = {
                millisecond: 1,
                second: 1E3,
                minute: 6E4,
                hour: 36E5,
                day: 864E5,
                week: 6048E5
            };
            if (d[b]) a._range = d[b] * c;
            else if (b === "month" || b === "year") a._range = {
                month: 30,
                year: 365
            } [b] * 864E5 * c
        },
        setInputValue: function(a, b) {
            var c = this.chart.options.rangeSelector;
            if (t(b)) this[a + "Input"].HCTime = b;
            this[a + "Input"].value = ua(c.inputEditDateFormat || "%Y-%m-%d", this[a + "Input"].HCTime);
            this[a + "DateBox"].attr({
                text: ua(c.inputDateFormat || "%b %e, %Y", this[a + "Input"].HCTime)
            })
        },
        //drawInput: function(a) {
        //    var b = this,
        //    c = b.chart,
        //    d = c.renderer.style,
        //    e = c.renderer,
        //    f = c.options.rangeSelector,
        //    g = b.div,
        //    h = a === "min",
        //    i, k, j, l = this.inputGroup;
        //    this[a + "Label"] = k = e.label(F.lang[h ? "rangeSelectorFrom": "rangeSelectorTo"], this.inputGroup.offset).attr({
        //        padding: 2
        //    }).css(y(d, f.labelStyle)).add(l);
        //    l.offset += k.width + 5;
        //    this[a + "DateBox"] = j = e.label("", l.offset).attr({
        //        padding: 2,
        //        width: f.inputBoxWidth || 90,
        //        height: f.inputBoxHeight || 17,
        //        stroke: f.inputBoxBorderColor || "silver",
        //        "stroke-width": 1
        //    }).css(y({
        //        textAlign: "center",
        //        color: "#444"
        //    },
        //    d, f.inputStyle)).on("click",
        //    function() {
        //        b[a + "Input"].focus()
        //    }).add(l);
        //    l.offset += j.width + (h ? 10 : 0);
        //    this[a + "Input"] = i = $("input", {
        //        name: a,
        //        className: "highcharts-range-selector",
        //        type: "text"
        //    },
        //    v({
        //        position: "absolute",
        //        border: 0,
        //        width: "1px",
        //        height: "1px",
        //        padding: 0,
        //        textAlign: "center",
        //        fontSize: d.fontSize,
        //        fontFamily: d.fontFamily,
        //        top: c.plotTop + "px"
        //    },
        //    f.inputStyle), g);
        //    i.onfocus = function() {
        //        E(this, {
        //            left: l.translateX + j.x + "px",
        //            top: l.translateY + "px",
        //            width: j.width - 2 + "px",
        //            height: j.height - 2 + "px",
        //            border: "2px solid silver"
        //        })
        //    };
        //    i.onblur = function() {
        //        E(this, {
        //            border: 0,
        //            width: "1px",
        //            height: "1px"
        //        });
        //        b.setInputValue(a)
        //    };
        //    i.onchange = function() {
        //        var a = i.value,
        //        d = (f.inputDateParser || Date.parse)(a),
        //        e = c.xAxis[0],
        //        g = e.dataMin,
        //        j = e.dataMax;
        //        isNaN(d) && (d = a.split("-"), d = Date.UTC(I(d[0]), I(d[1]) - 1, I(d[2])));
        //        isNaN(d) || (F.global.useUTC || (d += (new Date).getTimezoneOffset() * 6E4), h ? d > b.maxInput.HCTime ? d = r: d < g && (d = g) : d < b.minInput.HCTime ? d = r: d > j && (d = j), d !== r && c.xAxis[0].setExtremes(h ? d: e.min, h ? e.max: d, r, r, {
        //            trigger: "rangeSelectorInput"
        //        }))
        //    }
        //},
        render: function(a, b) {
            var c = this,
            d = c.chart,
            e = d.renderer,
            f = d.container,
            g = d.options,
            h = g.exporting && g.navigation && g.navigation.buttonOptions,
            i = g.rangeSelector,
            k = c.buttons,
            g = F.lang,
            j = c.div,
            j = c.inputGroup,
            l = i.buttonTheme,
            m = i.inputEnabled !== !1,
            n = l && l.states,
            p = d.plotLeft,
            r;
            if (!c.rendered && (c.zoomText = e.text(g.rangeSelectorZoom, p, d.plotTop - 20).css(i.labelStyle).add(), r = p + c.zoomText.getBBox().width + 5, q(c.buttonOptions,
            function(a, b) {
                k[b] = e.button(a.text, r, d.plotTop - 35,
                function() {
                    c.clickButton(b);
                    c.isActive = !0
                },
                l, n && n.hover, n && n.select).css({
                    textAlign: "center"
                }).add();
                r += k[b].width + o(i.buttonSpacing, 5);
                c.selected === b && k[b].setState(2)
            }), c.updateButtonStates(), m)) c.div = j = $("div", null, {
                position: "relative",
                height: 0,
                zIndex: 1
            }),
            f.parentNode.insertBefore(j, f),
            c.inputGroup = j = e.g("input-group").add(),
            j.offset = 0,
            c.drawInput("min"),
            c.drawInput("max");
            m && (f = d.plotTop - 45, j.align(v({
                y: f,
                width: j.offset,
                x: h && f < (h.y || 0) + h.height - d.spacing[0] ? -40 : 0
            },
            i.inputPosition), !0, d.spacingBox), c.setInputValue("min", a), c.setInputValue("max", b));
            c.rendered = !0
        },
        destroy: function() {
            var a = this.minInput,
            b = this.maxInput,
            c = this.chart,
            d = this.blurInputs,
            e;
            R(c.container, "mousedown", d);
            R(c, "resize", d);
            Ka(this.buttons);
            if (a) a.onfocus = a.onblur = a.onchange = null;
            if (b) b.onfocus = b.onblur = b.onchange = null;
            for (e in this) this[e] && e !== "chart" && (this[e].destroy ? this[e].destroy() : this[e].nodeType && Sa(this[e])),
            this[e] = null
        }
    };
    L.prototype.toFixedRange = function(a, b, c, d) {
        var e = this.chart && this.chart.fixedRange,
        a = o(c, this.translate(a, !0)),
        b = o(d, this.translate(b, !0)),
        c = e && (b - a) / e;
        c > 0.7 && c < 1.3 && (d ? a = b - e: b = a + e);
        return {
            min: a,
            max: b
        }
    };
    O(va.prototype, "init",
    function(a, b, c) {
        A(this, "init",
        function() {
            if (this.options.rangeSelector.enabled) this.rangeSelector = new zb(this)
        });
        a.call(this, b, c)
    });
    P.RangeSelector = zb;
    va.prototype.callbacks.push(function(a) {
        function b() {
            f = a.xAxis[0].getExtremes();
            g.render(f.min, f.max)
        }
        function c() {
            f = a.xAxis[0].getExtremes();
            isNaN(f.min) || h.render(f.min, f.max)
        }
        function d(a) {
            a.triggerOp !== "navigator-drag" && g.render(a.min, a.max)
        }
        function e(a) {
            h.render(a.min, a.max)
        }
        var f, g = a.scroller,
        h = a.rangeSelector;
        g && (A(a.xAxis[0], "afterSetExtremes", d), O(a, "drawChartBox",
        function(a) {
            var c = this.isDirtyBox;
            a.call(this);
            c && b()
        }), b());
        h && (A(a.xAxis[0], "afterSetExtremes", e), A(a, "resize", c), c());
        A(a, "destroy",
        function() {
            g && R(a.xAxis[0], "afterSetExtremes", d);
            h && (R(a, "resize", c), R(a.xAxis[0], "afterSetExtremes", e))
        })
    });
    P.StockChart = function(a, b) {
        var c = a.series,
        d, e = o(a.navigator && a.navigator.enabled, !0) ? {
            startOnTick: !1,
            endOnTick: !1
        }: null,
        f = {
            marker: {
                enabled: !1,
                radius: 2
            },
            states: {
                hover: {
                    lineWidth: 2
                }
            }
        },
        g = {
            shadow: !1,
            borderWidth: 0
        };
        a.xAxis = xa(ma(a.xAxis || {}),
        function(a) {
            return y({
                minPadding: 0,
                maxPadding: 0,
                ordinal: !0,
                title: {
                    text: null
                },
                labels: {
                    overflow: "justify"
                },
                showLastLabel: !0
            },
            a, {
                type: "datetime",
                categories: null
            },
            e)
        });
        a.yAxis = xa(ma(a.yAxis || {}),
        function(a) {
            d = o(a.opposite, !0);
            return y({
                labels: {
                    y: -2
                },
                opposite: d,
                showLastLabel: !1,
                title: {
                    text: null
                }
            },
            a)
        });
        a.series = null;
        a = y({
            chart: {
                panning: !0,
                pinchType: "x"
            },
            navigator: {
                enabled: !0
            },
            scrollbar: {
                enabled: !0
            },
            rangeSelector: {
                enabled: !0
            },
            title: {
                text: null,
                style: {
                    fontSize: "16px"
                }
            },
            tooltip: {
                shared: !0,
                crosshairs: !0
            },
            legend: {
                enabled: !1
            },
            plotOptions: {
                line: f,
                spline: f,
                area: f,
                areaspline: f,
                arearange: f,
                areasplinerange: f,
                column: g,
                columnrange: g,
                candlestick: g,
                ohlc: g
            }
        },
        a, {
            _stock: !0,
            chart: {
                inverted: !1
            }
        });
        a.series = c;
        return new va(a, b)
    };
    O(Xa.prototype, "init",
    function(a, b, c) {
        var d = c.chart.pinchType || "";
        a.call(this, b, c);
        this.pinchX = this.pinchHor = d.indexOf("x") !== -1;
        this.pinchY = this.pinchVert = d.indexOf("y") !== -1;
        this.hasZoom = this.hasZoom || this.pinchHor || this.pinchVert
    });
    O(L.prototype, "autoLabelAlign",
    function(a) {
        if (this.chart.options._stock && this.coll === "yAxis" && wa(this, this.chart.yAxis) === 0) {
            if (this.options.labels.x === 15) this.options.labels.x = 0;
            return "right"
        }
        return a.call(this)
    });
    L.prototype.getPlotLinePath = function(a, b, c, d, e) {
        var f = this,
        g = this.isLinked && !this.series ? this.linkedParent.series: this.series,
        h = f.chart,
        i = h.renderer,
        k = f.left,
        j = f.top,
        l,
        m,
        n,
        p,
        r = [],
        v,
        w;
        v = f.isXAxis ? t(f.options.yAxis) ? [h.yAxis[f.options.yAxis]] : xa(g,
        function(a) {
            return a.yAxis
        }) : t(f.options.xAxis) ? [h.xAxis[f.options.xAxis]] : xa(g,
        function(a) {
            return a.xAxis
        });
        q(f.isXAxis ? h.yAxis: h.xAxis,
        function(a) {
            if (t(a.options.id) ? a.options.id.indexOf("navigator") === -1 : 1) {
                var b = a.isXAxis ? "yAxis": "xAxis",
                b = t(a.options[b]) ? h[b][a.options[b]] : h[b][0];
                f === b && v.push(a)
            }
        });
        w = v.length ? [] : [f];
        q(v,
        function(a) {
            wa(a, w) === -1 && w.push(a)
        });
        e = o(e, f.translate(a, null, null, c));
        isNaN(e) || (f.horiz ? q(w,
        function(a) {
            m = a.top;
            p = m + a.len;
            l = n = s(e + f.transB); (l >= k && l <= k + f.width || d) && r.push("M", l, m, "L", n, p)
        }) : q(w,
        function(a) {
            l = a.left;
            n = l + a.width;
            m = p = s(j + f.height - e); (m >= j && m <= j + f.height || d) && r.push("M", l, m, "L", n, p)
        }));
        if (r.length > 0) return i.crispPolyLine(r, b || 1)
    };
    L.prototype.getPlotBandPath = function(a, b) {
        var c = this.getPlotLinePath(b),
        d = this.getPlotLinePath(a),
        e = [],
        f;
        if (d && c) for (f = 0; f < d.length; f += 6) e.push("M", d[f + 1], d[f + 2], "L", d[f + 4], d[f + 5], c[f + 4], c[f + 5], c[f + 1], c[f + 2]);
        else e = null;
        return e
    };
    ka.prototype.crispPolyLine = function(a, b) {
        var c;
        for (c = 0; c < a.length; c += 6) a[c + 1] === a[c + 4] && (a[c + 1] = a[c + 4] = s(a[c + 1]) - b % 2 / 2),
        a[c + 2] === a[c + 5] && (a[c + 2] = a[c + 5] = s(a[c + 2]) + b % 2 / 2);
        return a
    };
    if (Wa === P.VMLRenderer) fb.prototype.crispPolyLine = ka.prototype.crispPolyLine;
    O(L.prototype, "hideCrosshair",
    function(a, b) {
        a.call(this, b);
        t(this.crossLabelArray) && (t(b) ? this.crossLabelArray[b] && this.crossLabelArray[b].hide() : q(this.crossLabelArray,
        function(a) {
            a.hide()
        }))
    });
    O(L.prototype, "drawCrosshair",
    function(a, b, c) {
        var d, e;
        a.call(this, b, c);
        if (t(this.crosshair.label) && this.crosshair.label.enabled && t(c)) {
            var a = this.chart,
            f = this.options.crosshair.label,
            g = this.isXAxis ? "x": "y",
            b = this.horiz,
            h = this.opposite,
            i = this.left,
            k = this.top,
            j = this.crossLabel,
            l,
            m,
            n = f.format,
            p = "";
            if (!j) j = this.crossLabel = a.renderer.label().attr({
                align: f.align || (b ? "center": h ? this.labelAlign === "right" ? "right": "left": this.labelAlign === "left" ? "left": "center"),
                zIndex: 12,
                height: b ? 16 : r,
                fill: f.backgroundColor || this.series[0] && this.series[0].color || "gray",
                padding: o(f.padding, 2),
                stroke: f.borderColor || null,
                "stroke-width": f.borderWidth || 0
            }).css(v({
                color: "white",
                fontWeight: "normal",
                fontSize: "11px",
                textAlign: "center"
            },
            f.style)).add();
            b ? (l = c.plotX + i, m = k + (h ? 0 : this.height)) : (l = h ? this.width + i: 0, m = c.plotY + k);
            if (m < k || m > k + this.height) this.hideCrosshair();
            else { ! n && !f.formatter && (this.isDatetimeAxis && (p = "%b %d, %Y"), n = "{value" + (p ? ":" + p: "") + "}");
                j.attr({
                    text: n ? Ja(n, {
                        value: c[g]
                    }) : f.formatter.call(this, c[g]),
                    x: l,
                    y: m,
                    visibility: "visible"
                });
                c = j.getBBox();
                if (b) {
                    if (this.options.tickPosition === "inside" && !h || this.options.tickPosition !== "inside" && h) m = j.y - c.height
                } else m = j.y - c.height / 2;
                b ? (d = i - c.x, e = i + this.width - c.x) : (d = this.labelAlign === "left" ? i: 0, e = this.labelAlign === "right" ? i + this.width: a.chartWidth);
                j.translateX < d && (l += d - j.translateX);
                j.translateX + c.width >= e && (l -= j.translateX + c.width - e);
                j.attr({
                    x: l,
                    y: m,
                    visibility: "visible"
                })
            }
        }
    });
    var mc = ba.init,
    nc = ba.processData,
    oc = za.prototype.tooltipFormatter;
    ba.init = function() {
        mc.apply(this, arguments);
        this.setCompare(this.options.compare)
    };
    ba.setCompare = function(a) {
        this.modifyValue = a === "value" || a === "percent" ?
        function(b, c) {
            var d = this.compareValue;
            if (b !== r && (b = a === "value" ? b - d: b = 100 * (b / d) - 100, c)) c.change = b;
            return b
        }: null;
        if (this.chart.hasRendered) this.isDirty = !0
    };
    ba.processData = function() {
        var a = 0,
        b, c, d;
        nc.apply(this, arguments);
        if (this.xAxis && this.processedYData) {
            b = this.processedXData;
            c = this.processedYData;
            for (d = c.length; a < d; a++) if (typeof c[a] === "number" && b[a] >= this.xAxis.min) {
                this.compareValue = c[a];
                break
            }
        }
    };
    O(ba, "getExtremes",
    function(a) {
        a.call(this);
        if (this.modifyValue) this.dataMax = this.modifyValue(this.dataMax),
        this.dataMin = this.modifyValue(this.dataMin)
    });
    L.prototype.setCompare = function(a, b) {
        this.isXAxis || (q(this.series,
        function(b) {
            b.setCompare(a)
        }), o(b, !0) && this.chart.redraw())
    };
    za.prototype.tooltipFormatter = function(a) {
        a = a.replace("{point.change}", (this.change > 0 ? "+": "") + Ia(this.change, o(this.series.tooltipOptions.changeDecimals, 2)));
        return oc.apply(this, [a])
    };
    O(K.prototype, "render",
    function(a) {
        if (this.isCartesian && this.chart.options._stock) this.clipBox ? this.chart[this.sharedClipKey] && this.chart[this.sharedClipKey].attr({
            width: this.xAxis.len,
            height: this.yAxis.len
        }) : (this.clipBox = y(this.chart.clipBox), this.clipBox.width = this.xAxis.len, this.clipBox.height = this.yAxis.len);
        a.call(this)
    });
    v(P, {
        Axis: L,
        Chart: va,
        Color: Fa,
        Point: za,
        Tick: Za,
        Renderer: Wa,
        Series: K,
        SVGElement: Z,
        SVGRenderer: ka,
        arrayMin: Ra,
        arrayMax: Ba,
        charts: aa,
        dateFormat: ua,
        format: Ja,
        pathAnim: Bb,
        getOptions: function() {
            return F
        },
        hasBidiBug: Yb,
        isTouchDevice: cb,
        numberFormat: Ia,
        seriesTypes: C,
        setOptions: function(a) {
            F = y(!0, F, a);
            Mb();
            return F
        },
        addEvent: A,
        removeEvent: R,
        createElement: $,
        discardElement: Sa,
        css: E,
        each: q,
        extend: v,
        map: xa,
        merge: y,
        pick: o,
        splat: ma,
        extendClass: ga,
        pInt: I,
        wrap: O,
        svg: ca,
        canvas: ja,
        vml: !ca && !ja,
        product: "Highstock",
        version: "2.0.1"
    })
})();