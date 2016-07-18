require 'rest-client'

def test_getNext_emptyQueue_return404()
    RestClient.get('http://localhost:5000/api/jobitems/next') { |response, request, result, &block|
    case response.code
        when 404
            return true
        else
            puts "Unexpected response code: #{response.code}"
            return false
        end
    }
end

# def test_getNext2()
#     return false
# end