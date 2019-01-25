grammar SQL1;

@parser::members
{
	protected const int EOF = Eof;
}

@lexer::members
{
	protected const int EOF = Eof;
	protected const int HIDDEN = Hidden;
}

/*
 * Parser Rules
 */

compileUnit
	:	EOF
	;

/*
 * Lexer Rules
 */

WS
	:	' ' -> channel(HIDDEN)
	;

options { language = CSharp2; }

prog:		stmt+ ;

stmt:
			select_statement
/*
			|	insert_statement
			|	update_statement
			|	delete_statement
*/
			//	TODO: Other statements (e.g. IF)
			// Other 
			;

SELECT		:	[Ss][Ee][Ll][Ee][Cc][Tt] ;

DISTINCT	:	[Dd][Is][Ss][Tt][Ii][Nn][Cc][Tt] ;
			
TOP			:	[Tt][Oo][Pp] ;
	
FROM		:	[Ff][Rr][Oo][Mm] ;
			
top_clause:	 	TOP ('*' | '(*)' | INT | '(' INT ')')
			;
			
select_statement:
				SELECT 
				DISTINCT ?
				top_clause ?
				field_list ?
				from_clause ?
				// TODO: Add the following
				// where_clause?
				// orderby_clause?
				// groupby_clause?
			;
		
field_name:
				table_name ('.' table_name)? ;
			
field_list:		
				field_name (',' field_name)* ;

from_clause:								// Note: Incomplete. Needs JOIN Support
				FROM table_name
				;

expr:		expr ('*' | '/') expr
			|	expr ('+' | '-') expr
			|	INT
			|	ID
			|	'(' expr ')'
			;
	
// TODO: Get rid of this
KEYWORD:	[Ii][Nn][Ss][Ee][Rr][Tt]		// INSERT
			|	[Uu][Pp][Dd][Aa][Tt][Ee]	// UPDATE
			|	[Dd\[ee][Ll][Ee][Tt][Ee]	// DELETE
			// TODO: Add others here, such is IF, etc
			;
		
// TODO: Bad name, since we use it for a field name above
table_name:
			ID
			| '[' (ID | ' ')+ ']'
			;
			
ID		:		ALPHA (ALPHA | DIGIT)* ;
				
INT		:		'-'? [0-9]+;					
NL		:		'\r'? '\n' -> skip ;

STRING	:		'"' (ESC | .)*? '"' ;

WS		:		[ \t]+ -> skip ;			// Doesn't handle quoted strings

fragment
ESC		:		'\\' [btnr"\\] ; 

COMMENT	:		'/*' .*? '*/'	-> channel(HIDDEN)
		;
		
LINE_COMMENT
		:		'--' .*? NL		-> channel(HIDDEN)
		;
		
// Note: See common tokens on page 80

// Fragments

fragment
ALPHA	:		[a-zA-Z_] ;

fragment
DIGIT	:		[0-9];



