///remove_trailing_slash( string )

var _str = argument0;
var _length = string_length( _str );

switch( string_char_at( _str, _length ) ) {
    
    case "/":
    case "\":
        _str = string_copy( _str, 1, _length - 1 );
    break;
    
    default:
    break;
    
}

return _str;
