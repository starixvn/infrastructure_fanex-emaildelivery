var MultiAutoCompleteHelper = {
    trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    URLEncode: function (c) {
        var o = ''; var x = 0; c = c.toString(); var r = /(^[a-zA-Z0-9_.]*)/;
        while (x < c.length) {
            var m = r.exec(c.substr(x));
            if (m != null && m.length > 1 && m[1] != '') {
                o += m[1]; x += m[1].length;
            } else {
                if (c[x] == ' ') o += '+'; else {
                    var d = c.charCodeAt(x); var h = d.toString(16);
                    o += '%' + (h.length < 2 ? '0' : '') + h.toUpperCase();
                } x++;
            }
        } return o;
    },
    URLDecode: function (s) {
        // fixed -- + char decodes to space char
        var o = s;
        var binVal, t, b;
        var r = /(%[^%]{2}|\+)/;
        while ((m = r.exec(o)) != null && m.length > 1 && m[1] != '') {
            if (m[1] == '+') {
                t = ' ';
            } else {
                b = parseInt(m[1].substr(1), 16);
                t = String.fromCharCode(b);
            }
            o = o.replace(m[1], t);
        }
        return o;
    }
};
