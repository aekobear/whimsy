require 'sinatra'
require 'open-uri'

set :port, 80
set :bind, '0.0.0.0'

get '/' do
  open('http://api.ipify.org/?format=text') { |f| f.read }
end