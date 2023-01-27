grammar CommandGroup;

command: commandSegment+;

commandSegment: IDENT | '<' IDENT '>' | '[' IDENT ']';

IDENT: [a-zA-Z\u007f-\uffff][a-zA-Z0-9\u007f-\uffff]*;
WS: [ \t\r\n] -> skip;
